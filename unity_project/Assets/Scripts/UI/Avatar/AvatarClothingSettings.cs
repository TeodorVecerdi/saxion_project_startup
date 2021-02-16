using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AvatarClothingSettings : MonoBehaviour {
    [SerializeField] private Image ColorImage;
    [SerializeField] private Image RedSliderSelectedImage;
    [SerializeField] private Slider RedSlider;
    [SerializeField] private Image GreenSliderSelectedImage;
    [SerializeField] private Slider GreenSlider;
    [SerializeField] private Image BlueSliderSelectedImage;
    [SerializeField] private Slider BlueSlider;
    
    [Space, SerializeField] private List<Image> ClothingStyles;
    [SerializeField] private Image AvatarClothingStyle;

    public int SelectedClothingStyle;
    public Color ClothingColor;

    private void Start() {
        SelectClothingStyle(0);
        UpdateSliderColor();
    }

    public void OnRedChanged(float value) {
        UpdateSliderColor();
    }
    public void OnGreenChanged(float value) {
        UpdateSliderColor();
    }
    public void OnBlueChanged(float value) {
        UpdateSliderColor();
    }


    public void SelectClothingStyle(int index) {
        if (!Utils.RangeCheck(index, ClothingStyles.Count)) return;
        SelectedClothingStyle = index;

        AvatarClothingStyle.sprite = ClothingStyles[index].sprite;
    }

    private void UpdateSliderColor() {
        ClothingColor = new Color(RedSlider.value, GreenSlider.value, BlueSlider.value, 1f);
        RedSliderSelectedImage.color = new Color(RedSlider.value, 0f, 0f, 1f);
        GreenSliderSelectedImage.color = new Color(0f, GreenSlider.value, 0f, 1f);
        BlueSliderSelectedImage.color = new Color(0f, 0f, BlueSlider.value, 1f);
        ColorImage.color = ClothingColor;
        
        AvatarClothingStyle.color = ClothingColor;
        foreach (var graphic in ClothingStyles) {
            graphic.color = ClothingColor;
        }
    }
}

