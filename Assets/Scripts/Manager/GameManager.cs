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

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Block clickedBlock;
            if (boardRenderer.GetBlockFromPosition(mousePosition, out clickedBlock))
            {
                Debug.Log($"Clicked block at position: {clickedBlock.Position}");
            }
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            foreach(var block in board.GetBlocks())
            {
                Debug.Log("Block in " +block.Coordinates.x +" , " +block.Coordinates.y + "is at position " + block.Position ) ;
            }
        }
    }
}