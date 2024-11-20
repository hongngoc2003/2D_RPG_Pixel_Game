using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect/Thunder Explode")]

public class ThunderExplodeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderExplodePrefab;
    public override void ExecuteEffect(Transform _enemyPosition) {
        GameObject newThunderStrike = Instantiate(thunderExplodePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, .7f);

    }
}
