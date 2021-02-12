using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OnboardingController : MonoBehaviour {
    [SerializeField] private Image GradientImage;
    [SerializeField] private Image BackgroundImage;
    [SerializeField] private float ColorChangeDuration = 0.5f;
    [Space]
    [SerializeField] private Color PinkGradient;
    [SerializeField] private Color PinkBackground;
    [Space]
    [SerializeField] private Color GreenGradient;
    [SerializeField] private Color GreenBackground;
    [Space]
    [SerializeField] private Color BlueGradient;
    [SerializeField] private Color BlueBackground;
    [Space]
    [SerializeField] private Button RelationshipPreferenceButton;
    [SerializeField] private Button GenderPreferenceButton;
    [SerializeField] private Button AboutNextButton;
    
    private Gender gender = Gender.Male;
    private GenderPreference genderPreference = GenderPreference.None;
    private RelationshipPreference relationshipPreference = RelationshipPreference.None;
    private string name;
    private Date birthDate;
    private string about;

    public void OnGenderChange(int mask) {
        gender = (Gender) Enum.ToObject(typeof(Gender), mask);
    }
    
    public void OnRelationshipPreferenceChange(int mask) {
        relationshipPreference = (RelationshipPreference) Enum.ToObject(typeof(RelationshipPreference), mask);
        RelationshipPreferenceButton.interactable = relationshipPreference != RelationshipPreference.None;
    }

    public void OnGenderPreferenceChange(int mask) {
        genderPreference = (GenderPreference) Enum.ToObject(typeof(GenderPreference), mask);
        GenderPreferenceButton.interactable = genderPreference != GenderPreference.None;
    }

    public void OnNameChange(string newName) {
        name = newName;
        CheckAboutValid();
    }

    public void OnDayChange(string newDay) {
        birthDate.Day = newDay;
        CheckAboutValid();
    }
    
    public void OnMonthChange(string newMonth) {
        birthDate.Month = newMonth;
        CheckAboutValid();
    }
    
    public void OnYearChange(string newYear) {
        birthDate.Year = newYear;
        CheckAboutValid();
    }

    public void OnAboutChange(string newAbout) {
        about = newAbout;
    }
    
    private void CheckAboutValid() {
        var valid = !string.IsNullOrWhiteSpace(name) &&
                    !string.IsNullOrWhiteSpace(birthDate.Day) &&
                    !string.IsNullOrWhiteSpace(birthDate.Month) &&
                    !string.IsNullOrWhiteSpace(birthDate.Year);

        AboutNextButton.interactable = valid;
    }

    public void SetColorScheme(int scheme) {
        switch (scheme) {
            case 0:
                SwitchColor(PinkGradient, PinkBackground);
                break;
            case 1:
                SwitchColor(GreenGradient, GreenBackground);
                break;
            case 2:
                SwitchColor(BlueGradient, BlueBackground);
                break;
        }
    }

    private void SwitchColor(Color gradient, Color background) {
        GradientImage.DOColor(gradient, ColorChangeDuration);
        BackgroundImage.DOColor(background, ColorChangeDuration);
    }
}