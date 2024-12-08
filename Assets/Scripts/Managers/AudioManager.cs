using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    [SerializeField] private float sfxMinDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    public bool playBgm;
    private int bgmIndex;

    private void Awake() {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Update() {
        if (!playBgm)
            StopAllBGM();
        else {
            if (!bgm[bgmIndex].isPlaying)
                PlayBGM(bgmIndex);
        }
    }

    public void PlayRandomBGM() {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlaySFX(int _sfxIndex, Transform _source) {
        if(_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinDistance) 
            return;

        if (_sfxIndex < sfx.Length) {
            sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();

    public void PlayBGM(int _bgmIndex) {
        StopAllBGM();
        bgmIndex = _bgmIndex;
        bgm[bgmIndex].Play();
    }

    public void StopAllBGM() {
        for (int i = 0; i < bgm.Length; i++) {
            bgm[i].Stop();
        }
    }
}
