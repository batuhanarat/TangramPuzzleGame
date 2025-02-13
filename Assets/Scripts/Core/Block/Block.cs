using System;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    #region Private Variables

        private readonly int _x;
        private readonly int _y;
        private readonly Vector2Int _coordinates;
        private readonly Dictionary<TriangleType, bool> _triangleOccupancy;
        private readonly Dictionary<TriangleType, Triangle> _triangles;
        private readonly float _cellSize;
        private readonly Board _board;

    #endregion

    #region Properties

        public Vector3 Position { get; }
        public Vector2Int Coordinates => _coordinates;


        public Triangle UpperTriangle => _triangles[TriangleType.UP];
        public Triangle LowerTriangle => _triangles[TriangleType.DOWN];
        public Triangle RightTriangle => _triangles[TriangleType.RIGHT];
        public Triangle LeftTriangle => _triangles[TriangleType.LEFT];

    #endregion

    public Block(int x, int y, Vector3 boardCenter, float cellSize, int gridSize)
    {
        _board = ServiceProvider.Board;

        this._x = x;
        this._y = y;
        _coordinates = new Vector2Int(x, y);
        this._cellSize = cellSize;

        Position = CalculateWorldPosition(boardCenter, gridSize);
        Render(Position, cellSize);
        _triangleOccupancy = InitializeTriangleOccupancy();
        _triangles = InitializeTriangles();
    }

    private void Render(Vector3 position, float cellSize)
    {
        var blockRenderer = ServiceProvider.AssetLibrary.GetAsset<BlockRenderer>(AssetType.Block);
        blockRenderer.Configure(position,cellSize);
    }

    public void RemoveFromBlock(Triangle triangle)
    {
        if (triangle == null) return;
        _triangleOccupancy[triangle.TriangleType] = false;
    }

    public void OccupyTriangleInBlock(Triangle triangle)
    {
        if (triangle == null) return;
        _triangleOccupancy[triangle.TriangleType] = true;
    }

    public bool CheckCanAddToBlock(Triangle triangle)
    {
        if (triangle == null) return false;
        return !_triangleOccupancy[triangle.TriangleType];
    }

    public void InitBlock()
    {
        foreach (var type in Enum.GetValues(typeof(TriangleType)))
        {
            var triangleType = (TriangleType)type;
            var (left, right, opposite) = GetNeighborsForType(triangleType);
            _triangles[triangleType].SetNeighbors(left, right, opposite);
        }
    }

    private Vector3 CalculateWorldPosition(Vector3 boardCenter, int gridSize)
    {
        float offset = _cellSize * (gridSize - 1) / 2f;
        return boardCenter + new Vector3(
            (_x * _cellSize) - offset,
            (_y * _cellSize) - offset,
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
            triangleDict[type] = CreateAndConfigTriangle(type);
        }
        return triangleDict;
    }

    private Triangle CreateAndConfigTriangle(TriangleType type)
    {
        if(ServiceProvider.TriangleFactory == null)
        {
            Debug.Log("triangle factory null");
        }


        Triangle triangle = ServiceProvider.TriangleFactory.CreateTriangle(type, Position);

        SpriteRenderer sr = triangle.GetComponent<SpriteRenderer>();
        float scale = _cellSize / sr.sprite.bounds.size.x;
        triangle.transform.localScale = new Vector3(scale, scale, 1f);

        triangle.Init(_x, _y);

        _board.AddToAvailableTriangles(triangle);
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
            TriangleType.UP => _board.GetUpperBlock(_x, _y),
            TriangleType.DOWN => _board.GetBellowBlock(_x, _y),
            TriangleType.RIGHT => _board.GetRightBlock(_x, _y),
            TriangleType.LEFT => _board.GetLeftBlock(_x, _y),
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