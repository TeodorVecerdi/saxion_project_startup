using System;
using System.Collections.Generic;

[Serializable]
public class UserModel {
    public string Name;
    public Date BirthDate;
    public string About;
    public Gender Gender = Gender.Male;
    public List<int> GenrePreferences = new List<int>();
    public List<int> PlayedGames = new List<int>();
    public GenderPreference GenderPreference = GenderPreference.None;
    public RelationshipPreference RelationshipPreference = RelationshipPreference.None;
    public ProfilePictureType ProfilePictureType = ProfilePictureType.Avatar;
    public List<string> ProfilePictures;
    public AvatarConfiguration Avatar;
}

