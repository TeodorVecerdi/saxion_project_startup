using System;
using UnityCommons;
using UnityEngine;
using UnityEngine.UI;

public class ColorRandomizer : MonoBehaviour {
    [SerializeField] private Graphic TargetGraphic;
    [SerializeField] private Vector2 HueRange = Vector2.up;
    [SerializeField] private Vector2 SaturationRange = Vector2.up;
    [SerializeField] private Vector2 ValueRange = Vector2.up;

    private void Start() {
        var hue = Rand.Range(HueRange.x, HueRange.y);
        var sat = Rand.Range(SaturationRange.x, SaturationRange.y);
        var val = Rand.Range(ValueRange.x, ValueRange.y);
        TargetGraphic.color = Color.HSVToRGB(hue, sat, val);
    }
}