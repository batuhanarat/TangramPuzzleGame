using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject BoardPrefab;

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
        boardRenderer = Instantiate(BoardPrefab).GetComponent<BoardRenderer>();
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