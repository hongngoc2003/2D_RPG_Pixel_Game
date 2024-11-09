using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;

    private bool canGrow = true;
    private bool canShrink ;
    private bool canCreateHotKey = true;
    private bool cloneAttackReleased;

    private int amountOfAttack = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createHotKey = new List<GameObject>();

    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shinkSpeed, int _amountOfAttack, float _cloneAttackCooldown) {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shinkSpeed;
        amountOfAttack = _amountOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;
    }

    private void Update() {
        cloneAttackTimer -= Time.time;

        if (Input.GetKeyDown(KeyCode.F)) {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();


        if (canGrow && !canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack() {
        DestroyHotKey();
        cloneAttackReleased = true;
        canCreateHotKey = false;
    }

    private void CloneAttackLogic() {
        if (cloneAttackTimer < 0 && cloneAttackReleased) {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);
            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttack--;
            if (amountOfAttack <= 0) {
                canShrink = true;
                cloneAttackReleased = false;
            }
        }
    }

    private void DestroyHotKey() {
        if (createHotKey.Count <= 0)
            return;
        for (int i = 0; i < createHotKey.Count; i++) {
            Destroy(createHotKey[i]);
        }    

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.GetComponent<Enemy>() != null) {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
           if(collision.GetComponent<Enemy>() != null) {
            collision.GetComponent <Enemy>().FreezeTime(false);
        }
    }

    private void CreateHotKey(Collider2D collision) {
        if(keyCodeList.Count <= 0)
            return;

        if (!canCreateHotKey)
            return;
        


        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createHotKey.Add(newHotkey);

        KeyCode chosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chosenKey);

        BlackHoleHotkeyController newHotkeyScript = newHotkey.GetComponent<BlackHoleHotkeyController>();
        newHotkeyScript.SetupHotKey(chosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
