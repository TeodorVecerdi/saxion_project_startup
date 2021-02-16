using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AvatarEyeSettings : MonoBehaviour {
    [Space, SerializeField] private List<Image> EyeStyles;
    [SerializeField] private Image AvatarEyeStyle;

    private int selectedEyeStyle;

    private void Start() {
        SelectEyeStyle(0);
    }


    public void SelectEyeStyle(int index) {
        if (!Utils.RangeCheck(index, EyeStyles.Count)) return;
        selectedEyeStyle = index;

        AvatarEyeStyle.sprite = EyeStyles[index].sprite;
    }
}

