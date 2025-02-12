
using UnityEngine;
public interface ILevelReader
{
    public void PrepareNextLevel();
    public Level LoadLevel();
}
public class LevelReader : IProvidable, ILevelReader
{
    public int CurrentLevel { get; private set; }
    private const string LEVEL_KEY = "level";
    private const int TOTAL_LEVEL_COUNT = 10;
    public Level LevelData { get; set; }

    public LevelReader()
    {
        ServiceProvider.Register(this);
        SetCurrentLevel();
    }
    private void SetCurrentLevel()
    {
        if(!PlayerPrefs.HasKey(LEVEL_KEY)) {
            CurrentLevel = 1;
            PlayerPrefs.SetInt(LEVEL_KEY, CurrentLevel);
            PlayerPrefs.Save();
        } else {
            CurrentLevel = PlayerPrefs.GetInt(LEVEL_KEY);
        }
    }

    private void PersistLevel()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, CurrentLevel);
        PlayerPrefs.Save();
    }

    public void PrepareNextLevel()
    {
        CurrentLevel = (CurrentLevel + 1) % (TOTAL_LEVEL_COUNT+1);
        PersistLevel();
    }

    public Level LoadLevel()
    {
        string levelName = "level_"+CurrentLevel.ToString("D2");

        TextAsset levelFile = Resources.Load<TextAsset>($"Levels/{levelName}");

        if (levelFile != null)
        {
            return JsonUtility.FromJson<Level>(levelFile.text);
        } else {
            Debug.Log("Level file cant be found");
        }
        return default;
    }

}