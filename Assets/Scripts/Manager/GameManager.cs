using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject BoardPrefab;
    private BoardRenderer boardRenderer;
    private Board board;

    private int _columns;
    private int _rows;
    private int _seed;
    private int _pieceCount;

    private LevelSo level;
    public static GameManager Instance;


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
    }

    public void MoveToNextLevel()
    {
        level.NextLevel();
    }


    void Start()
    {
        boardRenderer = Instantiate(BoardPrefab).GetComponent<BoardRenderer>();
        board = Board.Instance;
        board.Init(_columns,_rows);
        board.CreateTangram(_seed,_pieceCount);
    }


}