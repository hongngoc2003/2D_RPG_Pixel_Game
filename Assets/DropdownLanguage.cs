using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownLanguage : MonoBehaviour
{
    public void DropdownLocale(int _index) {
        switch (_index) {
            case 0: LocaleSelector.instance.ChangeLocale(0); break;
            case 1: LocaleSelector.instance.ChangeLocale(1); break;
        }

    }
}
