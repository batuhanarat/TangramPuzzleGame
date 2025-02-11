using UnityEngine.SceneManagement;

public class LevelManager : IProvidable
{

    private LevelSo level;

    public LevelManager()
    {
        ServiceProvider.Register(this);
        level = ServiceProvider.Level;
    }
    public void LoadNextLevel()
    {
        level.NextLevel();
        SceneManager.LoadScene(0);
    }
}