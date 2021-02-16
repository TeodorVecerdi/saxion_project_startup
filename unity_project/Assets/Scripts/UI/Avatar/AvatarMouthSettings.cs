using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class AvatarMouthSettings : MonoBehaviour {
    [SerializeField] private Image ColorImage;
    [SerializeField] private Image RedSliderSelectedImage;
    [SerializeField] private Slider RedSlider;
    [SerializeField] private Image GreenSliderSelectedImage;
    [SerializeField] private Slider GreenSlider;
    [SerializeField] private Image BlueSliderSelectedImage;
    [SerializeField] private Slider BlueSlider;
    
    [Space, SerializeField] private List<Image> MouthStyles;
    [SerializeField] private Image AvatarMouthStyle;

    private int selectedMouthStyle;
    private Color mouthColor;

    private void Start() {
        SelectMouthStyle(0);
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


    public void SelectMouthStyle(int index) {
        if (!Utils.RangeCheck(index, MouthStyles.Count)) return;
        selectedMouthStyle = index;

        AvatarMouthStyle.sprite = MouthStyles[index].sprite;
    }

    private void UpdateSliderColor() {
        mouthColor = new Color(RedSlider.value, GreenSlider.value, BlueSlider.value, 1f);
        RedSliderSelectedImage.color = new Color(RedSlider.value, 0f, 0f, 1f);
        GreenSliderSelectedImage.color = new Color(0f, GreenSlider.value, 0f, 1f);
        BlueSliderSelectedImage.color = new Color(0f, 0f, BlueSlider.value, 1f);
        ColorImage.color = mouthColor;
        
        AvatarMouthStyle.color = mouthColor;
        foreach (var graphic in MouthStyles) {
            graphic.color = mouthColor;
        }
    }
}

