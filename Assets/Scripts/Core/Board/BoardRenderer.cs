using System.Collections.Generic;
using UnityEngine;

public class BoardRenderer : MonoBehaviour
{
    [Header("Board Settings")]

    public static BoardRenderer Instance;
    private int _gridSize;
    private float widthPercentage = 0.8f;
    private float heightPercentage = 0.4f;
    private float topOffsetPercentage = 0.1f;

    public float CellSize { get; private set; }
    public Vector3 BoardCenter { get; private set; }
    private Block[,] _blocks;

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
        _blocks = blocks;
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
            float originalCellSize = cellSprite.sprite.bounds.size.x;
            cell.transform.localScale = Vector3.one * (targetSize / originalCellSize);
        }
    }

    public bool GetBlockFromPosition(Vector3 worldPosition, out Block block)
    {
        Vector3 localPosition = worldPosition - BoardCenter;

        float gridOffset = (CellSize * (_gridSize - 1) / 2f);

        int col = Mathf.RoundToInt((localPosition.x + gridOffset) / CellSize);
        int row = Mathf.RoundToInt((localPosition.y + gridOffset) / CellSize);

        if (col < 0 || col >= _gridSize || row < 0 || row >= _gridSize)
        {
            block = default;
            return false;
        }

        block = _blocks[col, row];
        return true;
    }

}