using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeScreen : MonoBehaviour
{
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    public void FadeOut() => anim.SetTrigger("fadeOut");
    public void FadeIn() => anim.SetTrigger("fadeIn");

}
