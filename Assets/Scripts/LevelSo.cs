using UnityEngine;

[CreateAssetMenu(menuName = "BiggerGamesCase/LevelSo")]
public class LevelSo : ScriptableObject
{

    public int rows = 3;
    public int columns = 3;
    public int seed = 3;
    public int pieceCount = 3;

    public void NextLevel()
    {
        seed++;
    }


}

