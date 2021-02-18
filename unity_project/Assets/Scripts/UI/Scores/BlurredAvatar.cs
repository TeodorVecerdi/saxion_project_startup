using System;
using System.Collections.Generic;
using UnityCommons;
using UnityEngine;
using UnityEngine.UI;

public class BlurredAvatar : MonoBehaviour {
    [SerializeField] private List<Sprite> AvatarSprites;
    [SerializeField] private Image AvatarImage;

    private void Start() {
        AvatarImage.sprite = Rand.ListItem(AvatarSprites);
    }
}

