using System;
using System.Collections.Generic;

[Serializable]
public struct UserModel {
    public string Name;
    public Date BirthDate;
    public string About;
    public bool Gender;
    public GenderPreference GenderPreference;
    public RelationshipPreference RelationshipPreference;
    public List<int> GenrePreferences;
    public List<int> PlayedGames;
}

[Serializable]
public struct Date {
    public string Day;
    public string Month;
    public string Year;
}

public enum GenderPreference {
    Male,
    Female,
    Both
}

public enum RelationshipPreference {
    Friendship,
    RomanticRelationship,
    MeetNewPeople
}