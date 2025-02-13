using UnityEngine;

[CreateAssetMenu(menuName = "BiggerGamesCase/LevelConfig")]
public class LevelConfig : ScriptableObject
{

    public int boardSize = 4;
    public int seed = 3;
    public int pieceCount = 5;

    public void NextLevel()
    {
        seed++;
    }


}

