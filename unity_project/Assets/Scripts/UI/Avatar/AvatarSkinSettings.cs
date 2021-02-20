using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AvatarSkinSettings : MonoBehaviour {
    [SerializeField] private List<SkinColor> SkinColors;
    [SerializeField] private Image SliderBackgroundImage;
    [SerializeField] private Image SliderSelectedImage;
    [SerializeField] private Slider Slider;
    [SerializeField] private Image AvatarSkin;

    public int SelectedColor;
    public Color Color;

    private void Start() {
        foreach (var color in SkinColors) {
            color.Image.color = color.Color;
        }

        SelectColor(2);
    }

    public void OnSliderChanged(float value) {
        UpdateSliderColor();
    }

    public void SelectColor(int index) {
        if (!Utils.RangeCheck(index, SkinColors.Count)) return;
        SelectedColor = index;
        SetSliderColor();
        UpdateSliderColor();
    }

    private void SetSliderColor() {
        SliderBackgroundImage.color = SkinColors[SelectedColor].SliderMinColor;
    }

    private void UpdateSliderColor() {
        Color = Color.Lerp(SkinColors[SelectedColor].SliderMinColor, new Color(0, 0, 0, 1), Slider.value);
        SliderSelectedImage.color = Color;

        AvatarSkin.color = Color;
    }

    [Serializable] public class SkinColor {
        public Color Color;
        public Color SliderMinColor;
        public Image Image;
    } 
}

