using UnityEngine.SceneManagement;

public class LevelManager : IProvidable
{

    private LevelSo level;

    public LevelManager()
    {
        ServiceProvider.Register(this);
        level = ServiceProvider.Level;
    }
    public void PrepareNextLevel()
    {
        level.NextLevel();
        ServiceProvider.BoardRenderer.PlayWinAnimation(
            LoadNextLevel
            );
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(0);
    }
}