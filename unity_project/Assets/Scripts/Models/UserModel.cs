using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityCommons;
using UnityEngine;

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
            if (Gender == (Gender) genderValue) break;
            genderIdx++;
        }

        json["gender"] = genderIdx;
        json["genderPreference"] = new JArray(GetEnumIndices(GenderPreference).Cast<object>().ToArray());
        json["relationshipPreference"] = new JArray(GetEnumIndices(RelationshipPreference).Cast<object>().ToArray());
        json["genrePreference"] = new JArray(GetEnumIndices(GenrePreferences).Cast<object>().ToArray());
        json["playedGames"] = new JArray(GetEnumIndices(PlayedGames).Cast<object>().ToArray());
        json["profilePictureType"] = (int) ProfilePictureType - 1;
        if (ProfilePictureType == ProfilePictureType.Pictures)
            json["profilePictures"] = new JArray(ProfilePictures.Cast<object>().ToArray());
        if (ProfilePictureType == ProfilePictureType.Avatar)
            json["avatar"] = JsonConvert.SerializeObject(Avatar);

        return json.ToString(Formatting.None);
    }

    public static UserModel Deserialize(JToken user) {
        var userModel = new UserModel();
        
        userModel.Name = user.Value<string>("name");
        userModel.BirthDate = new Date();
        userModel.BirthDate.Day = user["birthDate"].Value<string>("day");
        userModel.BirthDate.Month = user["birthDate"].Value<string>("month");
        userModel.BirthDate.Year = user["birthDate"].Value<string>("year");

        userModel.About = user.Value<string>("about");
        userModel.Gender = GetEnumFromIndex<Gender>(user.Value<int>("gender"), 0);
        userModel.GenderPreference = IndicesToMask<GenderPreference>(user["genderPreference"].ToList().Select(token => token.Value<int>()));
        userModel.RelationshipPreference = IndicesToMask<RelationshipPreference>(user["relationshipPreference"].ToList().Select(token => token.Value<int>()));
        userModel.GenrePreferences = IndicesToMask<GameGenre>(user["genrePreference"].ToList().Select(token => token.Value<int>()));
        userModel.PlayedGames = IndicesToMask<GamePlayed>(user["playedGames"].ToList().Select(token => token.Value<int>()));
        userModel.ProfilePictureType = GetEnumFromIndex<ProfilePictureType>(user.Value<int>("profilePictureType"), 0);
        if (userModel.ProfilePictureType == ProfilePictureType.Pictures) {
            if (user.Value<string>("profilePictures") != "null")
                userModel.ProfilePictures = user.Values<string>("profilePictures").ToList();
            else userModel.ProfilePictures = null;
            userModel.Avatar = null;
        } else {
            userModel.ProfilePictures = null;
            if (user.Value<string>("avatar") != "null")
                userModel.Avatar = JsonConvert.DeserializeObject<AvatarConfiguration>(user.Value<string>("avatar"));
            else userModel.Avatar = null;
        }

        return userModel;
    }

    public static UserModel Deserialize(string json) {
        var user = JObject.Parse(json);
        return Deserialize(user["profile"]);
    }

    public static List<int> GetEnumIndices<T>(T enumValue) where T : Enum {
        var indices = new List<int>();
        var enumValues = Enum.GetValues(typeof(T)).Cast<object>().ToList();
        for (var i = 0; i < enumValues.Count; i++) {
            if ((int) enumValues[i] == 0) continue;
            if (enumValue.HasFlag((T) enumValues[i])) indices.Add(i - 1);
        }

        return indices;
    }

    private static T GetEnumFromIndex<T>(int index, int offset = 1) where T : Enum {
        var enumValues = Enum.GetValues(typeof(T)).Cast<object>().ToList();
        return (T) enumValues[index + offset];
    }

    private static T IndicesToMask<T>(IEnumerable<int> indices, int offset = 1) where T : Enum {
        var mask = (T)(object)0;
        var enumValues = Enum.GetValues(typeof(T)).Cast<object>().ToList();
        foreach (var i in indices) {
            var maskInt = (int) (object) mask;
            maskInt |= (int) enumValues[i + offset];
            mask = (T) (object) maskInt;
        }

        return mask;
    }
}