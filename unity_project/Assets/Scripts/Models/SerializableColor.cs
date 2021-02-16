using System;
using UnityEngine;

[Serializable]
public class SerializableColor {
    public float Red;
    public float Green;
    public float Blue;
    public float Alpha;

    private SerializableColor() { }

    public static SerializableColor FromColor(Color color) {
        return new SerializableColor {Red = color.r, Green = color.g, Blue = color.b, Alpha = color.a};
    }

    public static Color ToColor(SerializableColor color) {
        return new Color(color.Red, color.Green, color.Blue, color.Alpha);;
    }
    
    public static implicit operator Color (SerializableColor color) {
        return ToColor(color);
    }
    
    public static implicit operator SerializableColor (Color color) {
        return FromColor(color);
    }
}