using UnityEngine.SceneManagement;

public class LevelManager
{

    private LevelSo level;

    public LevelManager(LevelSo levelSO)
    {
        level = levelSO;
    }
    public void LoadNextLevel()
    {
        level.NextLevel();
        SceneManager.LoadScene(0);
    }
}