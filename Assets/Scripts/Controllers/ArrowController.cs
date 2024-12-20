using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private int dmg;
    [SerializeField] private string targetLayerName = "Player";

    [SerializeField] private float xVelocity;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove = true;

    private CharacterStats myStats;

    private void Update() {
        if (!canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    public void SetupArrow(float _speed, CharacterStats _myStats) {
        xVelocity = _speed;
        myStats = _myStats;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName)) {
            myStats.DoDamage(collision.GetComponent<CharacterStats>());

            StuckInto(collision);
        } else if(collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            StuckInto(collision);
        }
    }

    private void StuckInto(Collider2D collision) {
        GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(3, 4));
    }
}