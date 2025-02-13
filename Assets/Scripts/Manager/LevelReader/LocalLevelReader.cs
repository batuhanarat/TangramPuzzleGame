
using UnityEngine;
public interface ILevelReader
{
    public void PrepareNextLevel();
    public Level LoadLevel();
}
public class LocalLevelReader : BaseLevelReader, ILevelReader
{
    #region  Private Variables

        private const string LEVEL_KEY = "level";
        private const int TOTAL_LEVEL_COUNT = 12;

    #endregion

    #region Properties

    public int CurrentLevel { get; private set; }

    #endregion

    public LocalLevelReader() : base()
    {
        SetCurrentLevel();
    }

    private void SetCurrentLevel()
    {
        if(!PlayerPrefs.HasKey(LEVEL_KEY)) {
            CurrentLevel = 1;
            PlayerPrefs.SetInt(LEVEL_KEY, CurrentLevel);
            PlayerPrefs.Save();
        }
        else
        {
            CurrentLevel = PlayerPrefs.GetInt(LEVEL_KEY);
        }
    }

    private void PersistLevel()
    {
        PlayerPrefs.SetInt(LEVEL_KEY, CurrentLevel);
        PlayerPrefs.Save();
    }

    public override void PrepareNextLevel()
    {
        CurrentLevel = (CurrentLevel + 1) % (TOTAL_LEVEL_COUNT+1);
        PersistLevel();
    }

    public override Level LoadLevel()
    {
        string levelName = "level_"+CurrentLevel.ToString("D2");

        TextAsset levelFile = Resources.Load<TextAsset>($"Levels/{levelName}");

        if (levelFile != null)
        {
            LevelData =  JsonUtility.FromJson<Level>(levelFile.text);
            return LevelData;
        } else {
            Debug.Log("Level file cant be found");
        }
        return default;
    }

}