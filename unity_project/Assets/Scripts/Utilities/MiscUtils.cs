using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public static class MiscUtils {
    public static T RandomElement<T>(this List<T> list) {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static bool RandomBool() {
        return UnityEngine.Random.Range(0f, 1f) < 0.5f;
    }

    public static bool RandomBoolWeighted(float weight) {
        return UnityEngine.Random.Range(0f, 1f) < weight;
    }

    public static object ChooseBetween(object first, object second) {
        return RandomBool() ? first : second;
    }

    public static List<T> ChooseBetween<T>(List<T> first, List<T> second) {
        return RandomBool() ? first : second;
    }

    public static List<T> ChooseBetweenWithWeight<T>(List<T> first, List<T> second, float weight) {
        return UnityEngine.Random.Range(0f, 1f) <= weight ? first : second;
    }

    public static decimal MapRange(decimal value, decimal fromSource, decimal toSource, decimal fromTarget, decimal toTarget) {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }

    public static Vector3 MapRange(Vector3 value, Vector3 fromSource, Vector3 toSource, Vector3 fromTarget, Vector3 toTarget) {
        var x = (value.x - fromSource.x) / (toSource.x - fromSource.x) * (toTarget.x - fromTarget.x) + fromTarget.x;
        var y = (value.y - fromSource.y) / (toSource.y - fromSource.y) * (toTarget.y - fromTarget.y) + fromTarget.y;
        var z = (value.z - fromSource.z) / (toSource.z - fromSource.z) * (toTarget.z - fromTarget.z) + fromTarget.z;
        return new Vector3(x, y, z);
    }

    public static Vector2 MapRange(Vector2 value, Vector2 fromSource, Vector2 toSource, Vector2 fromTarget, Vector2 toTarget) {
        var x = (value.x - fromSource.x) / (toSource.x - fromSource.x) * (toTarget.x - fromTarget.x) + fromTarget.x;
        var y = (value.y - fromSource.y) / (toSource.y - fromSource.y) * (toTarget.y - fromTarget.y) + fromTarget.y;
        return new Vector2(x, y);
    }

    public static Texture2D ResizeTexture(Texture2D texture, int targetWidth, int targetHeight, bool alphaIsTransparency = false, bool mipChain = true) {
        var renderTexture = new RenderTexture(targetWidth, targetHeight, 32);
        RenderTexture.active = renderTexture;
        Graphics.Blit(texture, renderTexture);
        var result = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, mipChain);
#if UNITY_EDITOR // Unity is freaking out about this in the build
        result.alphaIsTransparency = alphaIsTransparency;
#endif
        result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        result.Apply();
        return result;
    }
}