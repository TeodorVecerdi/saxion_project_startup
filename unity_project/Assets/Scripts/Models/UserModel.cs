using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[Serializable]
public class UserModel {
    public string Name;
    public Date BirthDate;
    public string About;
    public Gender Gender = Gender.Male;
    public GenderPreference GenderPreference = GenderPreference.None;
    public RelationshipPreference RelationshipPreference = RelationshipPreference.None;
    public GameGenre GenrePreferences = GameGenre.None;
    public GamePlayed PlayedGames = GamePlayed.None;
    public ProfilePictureType ProfilePictureType = ProfilePictureType.Avatar;
    public List<string> ProfilePictures;
    public AvatarConfiguration Avatar;

    public string Serialize() {
        var json = new JObject();
        json["name"] = Name;
        json["birthDate"] = new JObject {["day"] = BirthDate.Day, ["month"] = BirthDate.Month, ["year"] = BirthDate.Year};
        json["about"] = About;

        int genderIdx = 0;
        var genderValues = Enum.GetValues(typeof(Gender));
        foreach (var genderValue in genderValues) {
            if(Gender == (Gender) genderValue) break;
            genderIdx++;
        }

        json["gender"] = genderIdx;
        json["genderPreferences"] = new JArray(GetEnumIndices(GenderPreference).Cast<object>().ToArray());
        json["relationshipPreference"] = new JArray(GetEnumIndices(RelationshipPreference).Cast<object>().ToArray());
        json["genrePreference"] = new JArray(GetEnumIndices(GenrePreferences).Cast<object>().ToArray());
        json["playedGames"] = new JArray(GetEnumIndices(PlayedGames).Cast<object>().ToArray());
        json["profilePictureType"] = (int) ProfilePictureType - 1;
        if(ProfilePictureType == ProfilePictureType.Pictures)
            json["profilePictures"] = new JArray(ProfilePictures.Cast<object>().ToArray());
        if(ProfilePictureType == ProfilePictureType.Avatar)
            json["avatar"] = JsonConvert.SerializeObject(Avatar);
        
        return json.ToString(Formatting.None);
    }

    private List<int> GetEnumIndices<T>(T enumValue) where T : Enum {
        var indices = new List<int>();
        var enumValues = Enum.GetValues(typeof(T)).Cast<object>().ToList();
        for (var i = 0; i < enumValues.Count; i++) {
            if((int)enumValues[i] == 0) continue;
            if(enumValue.HasFlag((T)enumValues[i])) indices.Add(i-1);
        }

        return indices;
    }
}

