using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private bool canAttack;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [Header("Duplicated Clone")]
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

    public void CreateCloneWithDelay(Transform _enemyTransform) {
        StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(1f * player.facingDir, 0)));
        
    }

    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset) {
        yield return new WaitForSeconds(.4f);
        CreateClone(_transform, _offset);
    }
}
