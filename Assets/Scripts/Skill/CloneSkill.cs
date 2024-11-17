using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private bool canAttack;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;

    [Header("Duplicated Clone")]
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Clone Crystal")]
    public bool cloneCrystal; 
    public void CreateClone(Transform _clonePosition, Vector3 _offset) {
        if(cloneCrystal) {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate,player);
    }

    public void CreateCloneOnDashBegin() {
        if(createCloneOnDashStart) {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver() {
        if (createCloneOnDashOver) {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform) {
        if(canCreateCloneOnCounterAttack) {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1f * player.facingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset) {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
