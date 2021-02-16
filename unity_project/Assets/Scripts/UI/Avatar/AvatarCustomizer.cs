using UnityEngine;

public class AvatarCustomizer : MonoBehaviour {
    [SerializeField] private AvatarSkinSettings SkinSettings;
    [SerializeField] private AvatarFacialSettings FacialSettings;
    [SerializeField] private AvatarEyebrowSettings EyebrowSettings;
    [SerializeField] private AvatarHairSettings HairSettings;
    [SerializeField] private AvatarEyeSettings EyeSettings;
    [SerializeField] private AvatarClothingSettings ClothingSettings;
    [SerializeField] private AvatarMouthSettings MouthSettings;
    [SerializeField] private AvatarAccessoriesSettings AccessorySettings;
    
    public AvatarConfiguration BuildAvatarModel() {
        return new AvatarConfiguration {
            SkinColor = SkinSettings.Color,
            FacialHairStyle = FacialSettings.SelectedHairStyle,
            FacialHairColor = FacialSettings.HairColor,
            EyebrowStyle = EyebrowSettings.SelectedEyebrowStyle,
            EyebrowColor = EyebrowSettings.Color,
            HairStyle = HairSettings.SelectedHairStyle,
            HairColor = HairSettings.Color,
            EyeStyle = EyeSettings.SelectedEyeStyle,
            ClothingStyle = ClothingSettings.SelectedClothingStyle,
            ClothingColor = ClothingSettings.ClothingColor,
            MouthStyle = MouthSettings.SelectedMouthStyle,
            MouthColor = MouthSettings.MouthColor,
            AccessoryStyle = AccessorySettings.SelectedAccessoryStyle,
            AccessoryColor = AccessorySettings.AccessoryColor
        };
    }
}

