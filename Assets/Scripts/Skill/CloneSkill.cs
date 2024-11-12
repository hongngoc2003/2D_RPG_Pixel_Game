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

    [SerializeField] private bool canCreateCloneOnCounterAttack;
    public void CreateClone(Transform _clonePosition, Vector3 _offset) {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform));
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
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1.5f * player.facingDir, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset) {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
