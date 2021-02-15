using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AvatarHairSettings : MonoBehaviour {
    [SerializeField] private List<HairColor> HairColors;
    [SerializeField] private Image SliderBackgroundImage;
    [SerializeField] private Image SliderSelectedImage;
    [SerializeField] private Slider Slider;
    [Space, SerializeField] private List<Image> HairStyles;
    [SerializeField] private Image AvatarHairStyle;

    private int selectedColor;
    private int selectedHairStyle;
    private Color hairColor;

    private void Start() {
        foreach (var color in HairColors) {
            color.Image.color = color.Color;
        }

        SelectColor(1);
        SelectHairStyle(2);
    }

    public void OnSliderChanged(float value) {
        UpdateSliderColor();
    }

    public void SelectColor(int index) {
        if (!Utils.RangeCheck(index, HairColors.Count)) return;
        selectedColor = index;
        SetSliderColor();
        UpdateSliderColor();
    }

    public void SelectHairStyle(int index) {
        if (!Utils.RangeCheck(index, HairStyles.Count)) return;
        selectedHairStyle = index;

        AvatarHairStyle.sprite = HairStyles[index].sprite;
    }

    private void SetSliderColor() {
        SliderBackgroundImage.color = HairColors[selectedColor].SliderMinColor;
    }

    private void UpdateSliderColor() {
        hairColor = Color.Lerp(HairColors[selectedColor].SliderMinColor, new Color(0, 0, 0, 1), Slider.value);
        SliderSelectedImage.color = hairColor;

        AvatarHairStyle.color = hairColor;
        foreach (var graphic in HairStyles) {
            graphic.color = hairColor;
        }
    }

    [Serializable] public class HairColor {
        public Color Color;
        public Color SliderMinColor;
        public Image Image;
    } 
}

