using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject BoardPrefab;
    private BoardRenderer boardRenderer;
    private Board board;

    public int rows = 3;
    public int columns = 3;
    public int seed = 3;
    public int pieceCount = 3;


    void Start()
    {
        boardRenderer = Instantiate(BoardPrefab).GetComponent<BoardRenderer>();
        board = Board.Instance;
        board.Init(columns,rows);
        board.CreateTangram(seed,pieceCount);
    }
}