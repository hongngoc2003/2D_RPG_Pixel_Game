using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public DashSkill dash {get; private set;}
    public CloneSkill clone { get; private set;}
    public SwordSkill sword { get; private set;}
    public BlackHoleSkill blackHole { get; private set;}
    public CrystalSkill crystal { get; private set;}
    public ParrySkill parry { get; private set;}
    public DodgeSkill dodge { get; private set;}
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject); // Hủy object mới nếu đã có instance
        } else {
            instance = this; // Gán instance mới
            DontDestroyOnLoad(gameObject); // Đảm bảo instance tồn tại xuyên scene
        }
    }
    private void Start() {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        sword = GetComponent<SwordSkill>();
        blackHole = GetComponent<BlackHoleSkill>();
        crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        dodge = GetComponent<DodgeSkill>();
    }
}
