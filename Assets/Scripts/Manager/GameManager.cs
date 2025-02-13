using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Private Variables

        private BoardRenderer _boardRenderer;
        private Board _board;
        private ITangramManager _tangramManager;
        private ILevelManager _levelManager;

    #endregion

    [Header("Game Settings")]
    [SerializeField] private GameConfig gameConfig;


    private void Awake()
    {
        _board = ServiceProvider.Board;
        _tangramManager = ServiceProvider.TangramManager;
        _levelManager = ServiceProvider.LevelManager;
    }

    private void Start()
    {
        _boardRenderer = ServiceProvider.AssetLibrary.GetAsset<BoardRenderer>(AssetType.Board);
        SetupGame();
        StartGame();
    }

    private void SetupGame()
    {
        _levelManager.SetupLevel();
        Random.InitState(_levelManager.Seed);
    }

    public void StartGame()
    {
        _board.Initialize(_levelManager.BoardSize, _levelManager.BoardSize);
        _tangramManager.CreateTangram(_levelManager.PieceCount);
    }


}