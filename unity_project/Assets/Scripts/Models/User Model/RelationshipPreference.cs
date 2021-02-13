[System.Flags]
public enum RelationshipPreference {
    None = 0,
    Friendship = 1 << 0,
    RomanticRelationship = 1 << 1,
    MeetNewPeople = 1 << 2
}