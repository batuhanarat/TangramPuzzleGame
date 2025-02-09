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
        board = new Board(columns, rows, seed, pieceCount);
        boardRenderer = Instantiate(BoardPrefab).GetComponent<BoardRenderer>();
        boardRenderer.AdjustBoard(columns,rows);

    }
}