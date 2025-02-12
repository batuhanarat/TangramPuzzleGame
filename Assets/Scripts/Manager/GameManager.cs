using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region Private Variables
        private BoardRenderer boardRenderer;
        private Board board;
        private ITangramManager tangramManager;
        private int _columns,_rows, _pieceCount;
        private LevelSo _level;

    #endregion

    private void Awake()
    {
        board = ServiceProvider.Board;
        tangramManager = ServiceProvider.TangramManager;
        _level = ServiceProvider.Level;
    }

    private void Start()
    {
        boardRenderer = ServiceProvider.AssetLibrary.GetAsset<BoardRenderer>(AssetType.Board);
        GetLevelData();
        StartGame();
    }

    private void GetLevelData()
    {
        _columns = _level.columns;
        _rows = _level.rows;
        _pieceCount = _level.pieceCount;
    }

    public void StartGame()
    {
        board.Initialize(_columns,_rows);
        tangramManager.CreateTangram(_pieceCount);
        //tangramManager.CreateTangramWithAnimation(_pieceCount);
    }


}