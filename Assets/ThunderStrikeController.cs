using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStat;
    [SerializeField] private float speed;
    private int dmg;

    private Animator anim;
    private bool triggered;

    void Start() {
        anim = GetComponentInChildren<Animator>();
    }
    
    public void Setup(int _dmg, CharacterStats _targetStat) {
        dmg = _dmg;
        targetStat = _targetStat;
    }

    void Update() {
        if (!targetStat)
            return;

        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStat.transform.position, speed * Time.deltaTime);
        transform.right = targetStat.transform.position - transform.position ;

        if (Vector2.Distance(transform.position, targetStat.transform.position) < .1f) {
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(0, .5f);

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DmgAndSelfDestroy", .2f);
            triggered = true;
            anim.SetTrigger("Hit");
        }
    }
    private void DmgAndSelfDestroy() {
        targetStat.ApplyShock(true);
        targetStat.TakeDamage(dmg);
        Destroy(gameObject, .4f);
    }
}
