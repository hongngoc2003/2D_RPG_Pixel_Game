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
    private int bounceAmount;
    private List<Transform> enemyTargets;
    private int targetIndex;

    [Header("Pierce info")]
    private int pierceAmount;

    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDir;
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

        SpinLogic();

    }

    private void SpinLogic() {
        if (isSpinning) {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped) {
                StopWhenSpinning();
            }

            if (wasStopped) {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDir, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0) {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0) {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders) {
                        if (hit.GetComponent<Enemy>() != null)
                            hit.GetComponent<Enemy>().Damage();
                    }
                }
            }
        }
    }

    private void StopWhenSpinning() {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuration;
    }

    private void BounceLogic() {
        if (isBouncing && enemyTargets.Count > 0) {

            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f) {
                enemyTargets[targetIndex].GetComponent<Enemy>().Damage();

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0) {
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

        if(pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDir = Mathf.Clamp(rb.velocity.x, -1, 1);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBOunce) {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBOunce;

        enemyTargets = new List<Transform>();
    }
    public void SetupPierce(int _pierceAmount) {
        pierceAmount = _pierceAmount;
    }
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCoolDown) {
        isSpinning = _isSpinning;
        spinDuration = _spinDuration;  
        maxTravelDistance = _maxTravelDistance;
        hitCooldown = _hitCoolDown;
    }

    public void ReturnSword() {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (isReturning)
            return;

        collision.GetComponent<Enemy>()?.Damage();

        SetupTargetsForBounce(collision);

        StuckInto(collision);
    }

    private void SetupTargetsForBounce(Collider2D collision) {
        if (collision.GetComponent<Enemy>() != null) {
            if (isBouncing && enemyTargets.Count <= 0) {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                foreach (var hit in colliders) {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTargets.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision) {
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null) {
            pierceAmount--;
            return;
        }

        if (isSpinning) {
            StopWhenSpinning();
            return;
        }

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
