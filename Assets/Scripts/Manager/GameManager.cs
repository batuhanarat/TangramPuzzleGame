using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Private Variables

        private BoardRenderer boardRenderer;
        private Board board;
        private ITangramManager tangramManager;
        private ILevelManager levelManager;
    #endregion

    [Header("Game Settings")]
    [SerializeField] private GameConfig gameConfig;



    private void Awake()
    {
        board = ServiceProvider.Board;
        tangramManager = ServiceProvider.TangramManager;
        levelManager = ServiceProvider.LevelManager;
    }

    private void Start()
    {
        boardRenderer = ServiceProvider.AssetLibrary.GetAsset<BoardRenderer>(AssetType.Board);
        SetupGame();
        StartGame();
    }

    private void SetupGame()
    {
        levelManager.SetupLevel();
        Random.InitState(levelManager.Seed);
    }

    public void StartGame()
    {
        board.Initialize(levelManager.BoardSize, levelManager.BoardSize);
        tangramManager.CreateTangram(levelManager.PieceCount);
    }


}