using System;

[Flags]
public enum GameGenre {
    None = 0,
    Action = 1 << 0,
    Adventure = 1 << 1,
    RolePlaying = 1 << 2,
    Simulation = 1 << 3,
    Strategy = 1 << 4,
    Sports = 1 << 5,
    MMO = 1 << 6,
    Sandbox = 1 << 7,
    Esports = 1 << 8,
    Casual = 1 << 9,
    Board = 1 << 10,
    Horror = 1 << 11
}