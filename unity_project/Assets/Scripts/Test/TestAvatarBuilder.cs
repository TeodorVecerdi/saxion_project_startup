using UnityEngine;
using UnityEngine.UI;

public class TestAvatarBuilder : MonoBehaviour {
    [SerializeField] private Image Skin;
    [SerializeField] private Image Clothing;
    [SerializeField] private Image Eyes;
    [SerializeField] private Image Eyebrows;
    [SerializeField] private Image Mouth;
    [SerializeField] private Image FacialHair;
    [SerializeField] private Image Hair;
    [SerializeField] private Image Accessories;

    private static AvatarSprites avatarSprites;

    public void Build(UserModel model) {
        if (avatarSprites == null)
            avatarSprites = Resources.Load<AvatarSprites>("Avatar Sprites");

        Skin.color = model.Avatar.SkinColor;
        
        Clothing.sprite = avatarSprites.Clothing[model.Avatar.ClothingStyle];
        Clothing.color = model.Avatar.ClothingColor;
        
        Eyes.sprite = avatarSprites.Eye[model.Avatar.EyeStyle];
        
        Eyebrows.sprite = avatarSprites.Eyebrow[model.Avatar.EyebrowStyle];
        Eyebrows.color = model.Avatar.EyebrowColor;
        
        Mouth.sprite = avatarSprites.Mouth[model.Avatar.MouthStyle];
        Mouth.color = model.Avatar.MouthColor;
        
        FacialHair.sprite = avatarSprites.FacialHair[model.Avatar.FacialHairStyle];
        FacialHair.color = model.Avatar.FacialHairColor;
        
        Hair.sprite = avatarSprites.Hair[model.Avatar.HairStyle];
        Hair.color = model.Avatar.HairColor;
        
        Accessories.sprite = avatarSprites.Accessory[model.Avatar.AccessoryStyle];
        Accessories.color = model.Avatar.AccessoryColor;
    }
}

