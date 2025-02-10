using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [Header("Board Settings")]

    public static BoardRenderer Instance;
    private int _gridSize;
    public float widthPercentage = 0.8f;
    public float heightPercentage = 0.4f;
    public float topOffsetPercentage = 0.2f;

    public float CellSize { get; private set; }
    public Vector3 BoardCenter { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AdjustBoard(int columns, int rows, Block[,] blocks)
    {
        _gridSize = columns;
        InitializeBoard(columns, rows, blocks);
    }


    private void InitializeBoard(int columns, int rows, Block[,] blocks)
    {
        Camera mainCamera = Camera.main;
        float screenHeight = 2f * mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        float boardWidth = screenWidth * widthPercentage;
        float boardHeight = screenHeight * heightPercentage;
        float boardSize = Mathf.Min(boardWidth, boardHeight);

        float topOffset = screenHeight * topOffsetPercentage;
        float boardTopEdgeY = mainCamera.orthographicSize - topOffset;
        BoardCenter = new Vector3(0f, boardTopEdgeY - (boardSize / 2f), 0f);

        SpriteRenderer boardSprite = GetComponent<SpriteRenderer>();
        if (boardSprite != null && boardSprite.sprite != null)
        {
            float originalSize = boardSprite.sprite.bounds.size.x;
            transform.localScale = Vector3.one * (boardSize / originalSize);
        }

        transform.position = BoardCenter;

        float innerArea = boardSize * 0.96f;
        CellSize = innerArea / _gridSize;

        for(int col = 0; col < columns ; col++ )
        {
            for(int row = 0 ; row < rows ; row++)
            {
                var block = new Block(col, row, BoardCenter, CellSize, _gridSize );
                blocks[col,row] = block;

            }
        }
    }


    private void ScaleCellToSize(GameObject cell, float targetSize)
    {
        SpriteRenderer cellSprite = cell.GetComponent<SpriteRenderer>();
        if (cellSprite != null && cellSprite.sprite != null)
        {
            float originalCellSize = cellSprite.sprite.bounds.size.x; // Hücre sprite'ının orijinal boyutu
            cell.transform.localScale = Vector3.one * (targetSize / originalCellSize);
        }
    }
}