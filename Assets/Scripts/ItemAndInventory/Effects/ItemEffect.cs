using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class ItemEffect : ScriptableObject
{
    [SerializeField] public LocalizedString effectDescription;

    public virtual void ExecuteEffect(Transform _enemyPosition) {
        Debug.Log("Effect executed");
    }
}
