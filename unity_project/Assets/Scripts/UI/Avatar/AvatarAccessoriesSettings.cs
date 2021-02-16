using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AvatarAccessoriesSettings : MonoBehaviour {
    [SerializeField] private Image ColorImage;
    [SerializeField] private Image RedSliderSelectedImage;
    [SerializeField] private Slider RedSlider;
    [SerializeField] private Image GreenSliderSelectedImage;
    [SerializeField] private Slider GreenSlider;
    [SerializeField] private Image BlueSliderSelectedImage;
    [SerializeField] private Slider BlueSlider;
    
    [Space, SerializeField] private List<Image> AccessoryStyles;
    [SerializeField] private Image AvatarAccessoryStyle;

    public int SelectedAccessoryStyle;
    public Color AccessoryColor;

    private void Start() {
        SelectAccessoryStyle(0);
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


    public void SelectAccessoryStyle(int index) {
        if (!Utils.RangeCheck(index, AccessoryStyles.Count)) return;
        SelectedAccessoryStyle = index;

        AvatarAccessoryStyle.sprite = AccessoryStyles[index].sprite;
    }

    private void UpdateSliderColor() {
        AccessoryColor = new Color(RedSlider.value, GreenSlider.value, BlueSlider.value, 1f);
        RedSliderSelectedImage.color = new Color(RedSlider.value, 0f, 0f, 1f);
        GreenSliderSelectedImage.color = new Color(0f, GreenSlider.value, 0f, 1f);
        BlueSliderSelectedImage.color = new Color(0f, 0f, BlueSlider.value, 1f);
        ColorImage.color = AccessoryColor;
        
        AvatarAccessoryStyle.color = AccessoryColor;
        foreach (var graphic in AccessoryStyles) {
            graphic.color = AccessoryColor;
        }
    }
}

