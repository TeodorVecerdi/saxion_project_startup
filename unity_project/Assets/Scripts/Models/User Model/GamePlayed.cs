using System;

[Flags]
public enum GamePlayed {
    None = 0,
    League = 1 << 0,
    Rust = 1 << 1,
    GTA = 1 << 2,
    Fortnite = 1 << 3,
    COD = 1 << 4,
    Apex = 1 << 5,
    CSGO = 1 << 6,
    Valorant = 1 << 7
}