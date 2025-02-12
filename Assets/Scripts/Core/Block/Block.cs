using System;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    #region Private Variables

        private readonly int x;
        private readonly int y;
        private readonly Vector2Int coordinates;
        private readonly Dictionary<TriangleType, bool> triangleOccupancy;
        private readonly Dictionary<TriangleType, Triangle> triangles;
        private readonly float cellSize;
        private readonly Board board;

    #endregion

    #region Properties

        public Vector3 Position { get; }
        public Vector2Int Coordinates => coordinates;


        public Triangle UpperTriangle => triangles[TriangleType.UP];
        public Triangle LowerTriangle => triangles[TriangleType.DOWN];
        public Triangle RightTriangle => triangles[TriangleType.RIGHT];
        public Triangle LeftTriangle => triangles[TriangleType.LEFT];


        public bool IsLeftTriangleOccupied => triangleOccupancy[TriangleType.LEFT];
        public bool IsRightTriangleOccupied => triangleOccupancy[TriangleType.RIGHT];
        public bool IsUpperTriangleOccupied => triangleOccupancy[TriangleType.UP];
        public bool IsLowerTriangleOccupied => triangleOccupancy[TriangleType.DOWN];

    #endregion

    public Block(int x, int y, Vector3 boardCenter, float cellSize, int gridSize)
    {
        board = ServiceProvider.Board;

        this.x = x;
        this.y = y;
        coordinates = new Vector2Int(x, y);
        this.cellSize = cellSize;
        Position = CalculateWorldPosition(boardCenter, gridSize);
        Render(Position, cellSize);
        triangleOccupancy = InitializeTriangleOccupancy();
        triangles = InitializeTriangles();
    }

    private void Render(Vector3 position, float cellSize)
    {
        var blockRenderer = ServiceProvider.AssetLibrary.GetAsset<BlockRenderer>(AssetType.Block);
        blockRenderer.Configure(position,cellSize);
    }

    public void RemoveFromBlock(Triangle triangle)
    {
        if (triangle == null) return;
        triangleOccupancy[triangle.TriangleType] = false;
    }

    public void OccupyTriangleInBlock(Triangle triangle)
    {
        if (triangle == null) return;
        triangleOccupancy[triangle.TriangleType] = true;
    }

    public bool CheckCanAddToBlock(Triangle triangle)
    {
        if (triangle == null) return false;
        return !triangleOccupancy[triangle.TriangleType];
    }

    public void InitBlock()
    {
        foreach (var type in Enum.GetValues(typeof(TriangleType)))
        {
            var triangleType = (TriangleType)type;
            var (left, right, opposite) = GetNeighborsForType(triangleType);
            triangles[triangleType].SetNeighbors(left, right, opposite);
        }
    }

    private Vector3 CalculateWorldPosition(Vector3 boardCenter, int gridSize)
    {
        float offset = cellSize * (gridSize - 1) / 2f;
        return boardCenter + new Vector3(
            (x * cellSize) - offset,
            (y * cellSize) - offset,
            0f
        );
    }

    private Dictionary<TriangleType, bool> InitializeTriangleOccupancy()
    {
        var occupancy = new Dictionary<TriangleType, bool>();
        foreach (TriangleType type in Enum.GetValues(typeof(TriangleType)))
        {
            occupancy[type] = false;
        }
        return occupancy;
    }

    private Dictionary<TriangleType, Triangle> InitializeTriangles()
    {
        var triangleDict = new Dictionary<TriangleType, Triangle>();
        foreach (TriangleType type in Enum.GetValues(typeof(TriangleType)))
        {
            triangleDict[type] = CreateTriangle(type);
        }
        return triangleDict;
    }

    private Triangle CreateTriangle(TriangleType type)
    {

        Triangle triangle = ServiceProvider.TriangleFactory.GetTriangleFromType(type, Position);

        SpriteRenderer sr = triangle.GetComponent<SpriteRenderer>();
        float scale = cellSize / sr.sprite.bounds.size.x;
        triangle.transform.localScale = new Vector3(scale, scale, 1f);

        triangle.Init(x, y);

        board.AddToAvailableTriangles(triangle);
        return triangle;
    }

    private (Triangle left, Triangle right, Triangle opposite) GetNeighborsForType(TriangleType type)
    {
        return type switch
        {
            TriangleType.UP => (LeftTriangle, RightTriangle, GetNeighborTriangle(type)),
            TriangleType.DOWN => (LeftTriangle, RightTriangle, GetNeighborTriangle(type)),
            TriangleType.RIGHT => (UpperTriangle, LowerTriangle, GetNeighborTriangle(type)),
            TriangleType.LEFT => (UpperTriangle, LowerTriangle, GetNeighborTriangle(type)),
            _ => throw new ArgumentException($"Invalid triangle type: {type}")
        };
    }

    private Triangle GetNeighborTriangle(TriangleType type)
    {
        var neighborBlock = type switch
        {
            TriangleType.UP => board.GetUpperBlock(x, y),
            TriangleType.DOWN => board.GetBellowBlock(x, y),
            TriangleType.RIGHT => board.GetRightBlock(x, y),
            TriangleType.LEFT => board.GetLeftBlock(x, y),
            _ => throw new ArgumentException($"Invalid triangle type: {type}")
        };

        return neighborBlock?.GetOppositeTriangle(type);
    }

    private Triangle GetOppositeTriangle(TriangleType type) => type switch
    {
        TriangleType.UP => LowerTriangle,
        TriangleType.DOWN => UpperTriangle,
        TriangleType.RIGHT => LeftTriangle,
        TriangleType.LEFT => RightTriangle,
        _ => throw new ArgumentException($"Invalid triangle type: {type}")
    };


}