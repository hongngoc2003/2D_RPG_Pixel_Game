using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D circleCol;
    private Player player;

    [SerializeField] private float returnSpeed = 12;
    private bool canRotate = true;
    private bool isReturning;

    [Header("Bounce info")]
    [SerializeField] private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounce;
    private List<Transform> enemyTargets;
    private int targetIndex;


    private void Awake() {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCol = GetComponent<CircleCollider2D>();
    }
    private void Update() {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning) {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchSword();
        }

        BounceLogic();

    }

    private void BounceLogic() {
        if (isBouncing && enemyTargets.Count > 0) {

            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f) {
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0) {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player) {
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        player = _player;

        anim.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBOunce) {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBOunce;

        enemyTargets = new List<Transform>();
    }
    public void ReturnSword() {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null) {
            if (isBouncing && enemyTargets.Count <= 0) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders) {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTargets.Add(hit.transform);
                }
            }
        }

        StuckInto(collision);
    }

    private void StuckInto(Collider2D collision) {

        canRotate = false;
        circleCol.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargets.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
