using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D circleCol => GetComponent<CircleCollider2D>();
    private Player player;

    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform closestEnemy;
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy, Player _player) {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
        player = _player;
    }

    private void Update() {
        crystalExistTimer -= Time.deltaTime;
        if(crystalExistTimer < 0) {
            CrystalLogic();
        }

        if(canMove) {
            if (closestEnemy == null)
                return;

            transform.position = Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, closestEnemy.position) < 1) {
                CrystalLogic();
                canMove = false;
            }
        }

        if(canGrow) {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    public void ChooseRandomEnemy() {
        float radius = SkillManager.instance.blackHole.GetBlackHoleRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if(colliders.Length > 0)
            closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
    }
    private void AnimationExplodeEvent() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCol.radius);
        foreach (var hit in colliders) {
            if (hit.GetComponent<Enemy>() != null) {

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                player.stats.DoMagicalDmg(hit.GetComponent<CharacterStats>());

                ItemDataEquipment equippedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
                if(equippedAmulet != null) {
                    equippedAmulet.ExecuteItemEffects(hit.transform);
                }
            }

        }
    }

        public void CrystalLogic() {
        if (canExplode) {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);

}
