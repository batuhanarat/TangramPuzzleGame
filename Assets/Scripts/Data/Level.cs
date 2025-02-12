
using System;

[Serializable]
public class Level
{
    public int Seed;
    public int BoardSize;
    public int PieceCount;
}

public enum Difficulty
{
    INVALID = -1,
    EASY = 0,
    MEDIUM = 1,
    HARD = 2
}