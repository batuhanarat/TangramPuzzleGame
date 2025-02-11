using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject BoardPrefab;

    #region Private Variables
        private BoardRenderer boardRenderer;
        private Board board;
        private int _columns;
        private int _rows;
        private int _seed;
        private int _pieceCount;
        private LevelSo level;
        private TangramManager tangramManager;

    #endregion

    #region Properties

        public static GameManager Instance;

    #endregion


    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        level = Resources.Load<LevelSo>("ScriptableObjects/LevelConfig");
        _columns = level.columns;
        _rows = level.rows;
        _seed = level.seed;
        _pieceCount = level.pieceCount;
        UnityEngine.Random.InitState(_seed);
    }


    void Start()
    {
        boardRenderer = Instantiate(BoardPrefab).GetComponent<BoardRenderer>();
        board = Board.Instance;

        board.Initialize(_columns,_rows);

        tangramManager.CreateTangram(_pieceCount);
    }


}