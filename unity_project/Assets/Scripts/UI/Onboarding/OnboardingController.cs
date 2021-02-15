using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using RestSharp;
using UnityEngine;
using UnityEngine.UI;

public class OnboardingController : MonoBehaviour {
    [Foldout("Color Scheme"), SerializeField] private Image GradientImage;
    [Foldout("Color Scheme"), SerializeField] private Image BackgroundImage;
    [Foldout("Color Scheme"), SerializeField] private float ColorChangeDuration = 0.5f;
    [Foldout("Color Scheme"), Space, SerializeField] private Color PinkGradient;
    [Foldout("Color Scheme"), SerializeField] private Color PinkBackground;
    [Foldout("Color Scheme"), Space, SerializeField] private Color GreenGradient;
    [Foldout("Color Scheme"), SerializeField] private Color GreenBackground;
    [Foldout("Color Scheme"), Space, SerializeField] private Color BlueGradient;
    [Foldout("Color Scheme"), SerializeField] private Color BlueBackground;
    [Space]
    [SerializeField] private Button RelationshipPreferenceButton;
    [SerializeField] private Button GenderPreferenceButton;
    [SerializeField] private Button AboutNextButton;
    [SerializeField] private Button GameGenreButton;
    [SerializeField] private Button GamesPlayedButton;

    private Gender gender = Gender.Male;
    private GenderPreference genderPreference = GenderPreference.None;
    private RelationshipPreference relationshipPreference = RelationshipPreference.None;
    private string name;
    private Date birthDate;
    private string about;
    private GameGenre gameGenres = GameGenre.None;
    private GamePlayed gamesPlayed = GamePlayed.None;
    private ProfilePictureType profilePictureType = ProfilePictureType.Avatar;

    public void Finish() {
        var userModel = new UserModel {
            Name = name,
            BirthDate = birthDate,
            About = about,
            Gender = gender,
            GenderPreference = genderPreference,
            RelationshipPreference = relationshipPreference,
            GenrePreferences = gameGenres,
            PlayedGames = gamesPlayed,
            ProfilePictureType = profilePictureType,
            ProfilePictures = new List<string>(),
            Avatar = null
        };
        var json = userModel.Serialize();
        ServerConnection.Instance.MakeRequestAsync("/profile", Method.POST, new List<(string key, string value)> {("id", UserState.Instance.UserId), ("profile", json)}, response => {
            Debug.Log($"{response.IsSuccessful}\n{response.StatusCode}\n{response.StatusDescription}\n{response.Content}\n{response.ErrorMessage}");
        });
    }

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

    public void OnGameGenreChange(int mask) {
        gameGenres = (GameGenre) Enum.ToObject(typeof(GameGenre), mask);
        GameGenreButton.interactable = gameGenres != GameGenre.None;
    }
    
    public void OnGamesPlayedChange(int mask) {
        gamesPlayed = (GamePlayed) Enum.ToObject(typeof(GamePlayed), mask);
        GamesPlayedButton.interactable = gamesPlayed != GamePlayed.None;
    }
    
    public void OnProfilePictureTypeChange(int mask) {
        profilePictureType = (ProfilePictureType) Enum.ToObject(typeof(ProfilePictureType), mask);
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