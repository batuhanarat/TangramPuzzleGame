using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [Header("Board Settings")]
    public GameObject trianglePrefab;
    private int _gridSize;
    public float widthPercentage = 0.8f;
    public float heightPercentage = 0.4f;
    public float topOffsetPercentage = 0.2f;

    public float CellSize { get; private set; }
    public Vector3 BoardCenter { get; private set; }


    public void AdjustBoard(int columns, int rows)
    {
        _gridSize = columns;
        InitializeBoard();
    }

    private void InitializeBoard()
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