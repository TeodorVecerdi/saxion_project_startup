using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.UI;

public class AvatarEyebrowSettings : MonoBehaviour {
    [SerializeField] private List<EyebrowColor> EyebrowColors;
    [SerializeField] private Image SliderBackgroundImage;
    [SerializeField] private Image SliderSelectedImage;
    [SerializeField] private Slider Slider;
    [Space, SerializeField] private List<Image> EyebrowStyles;
    [SerializeField] private Image AvatarEyebrowStyle;

    private int selectedColor;
    private int selectedEyebrowStyle;
    private Color eyebrowColor;

    private void Start() {
        foreach (var color in EyebrowColors) {
            color.Image.color = color.Color;
        }

        SelectColor(1);
        SelectEyebrowStyle(3);
    }

    public void OnSliderChanged(float value) {
        UpdateSliderColor();
    }

    public void SelectColor(int index) {
        if (!Utils.RangeCheck(index, EyebrowColors.Count)) return;
        selectedColor = index;
        SetSliderColor();
        UpdateSliderColor();
    }

    public void SelectEyebrowStyle(int index) {
        if (!Utils.RangeCheck(index, EyebrowStyles.Count)) return;
        selectedEyebrowStyle = index;

        AvatarEyebrowStyle.sprite = EyebrowStyles[index].sprite;
    }

    private void SetSliderColor() {
        SliderBackgroundImage.color = EyebrowColors[selectedColor].SliderMinColor;
    }

    private void UpdateSliderColor() {
        eyebrowColor = Color.Lerp(EyebrowColors[selectedColor].SliderMinColor, new Color(0, 0, 0, 1), Slider.value);
        SliderSelectedImage.color = eyebrowColor;

        AvatarEyebrowStyle.color = eyebrowColor;
        foreach (var graphic in EyebrowStyles) {
            graphic.color = eyebrowColor;
        }
    }

    [Serializable] public class EyebrowColor {
        public Color Color;
        public Color SliderMinColor;
        public Image Image;
    } 
}

