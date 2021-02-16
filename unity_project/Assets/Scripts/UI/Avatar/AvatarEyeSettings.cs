using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AvatarEyeSettings : MonoBehaviour {
    [Space, SerializeField] private List<Image> EyeStyles;
    [SerializeField] private Image AvatarEyeStyle;

    public int SelectedEyeStyle;

    private void Start() {
        SelectEyeStyle(0);
    }


    public void SelectEyeStyle(int index) {
        if (!Utils.RangeCheck(index, EyeStyles.Count)) return;
        SelectedEyeStyle = index;

        AvatarEyeStyle.sprite = EyeStyles[index].sprite;
    }
}

