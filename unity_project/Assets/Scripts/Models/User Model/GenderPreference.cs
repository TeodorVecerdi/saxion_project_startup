[System.Flags]
public enum GenderPreference {
    None = 0,
    Male = 1 << 0,
    Female = 1 << 1,
    Other = 1 << 2,
    Everyone = 1 << 3
}