using System.Collections.Generic;

public class Board : IProvidable
{

    #region Private Variables

        private Block[,] _blocks;
        private int _columns;
        private int _rows;
        private readonly List<Triangle> _availableTriangles = new();

    #endregion

    public Board()
    {
        ServiceProvider.Register(this);
    }

    public void AddToAvailableTriangles(Triangle triangle)
    {
        _availableTriangles.Add(triangle);
    }

    public void RemoveFromAvailableTriangels(Triangle triangle)
    {
        _availableTriangles.Remove(triangle);
    }

    public Triangle GetRandomTriangle()
    {
        var randomIndex = UnityEngine.Random.Range(0,_availableTriangles.Count);
        return _availableTriangles[randomIndex];
    }

    public void Initialize(int columns, int rows)
    {
        InitializeBoard(columns, rows);
        InitializeBlocks();
    }

    private void InitializeBoard(int columns, int rows)
    {
        _columns = columns;
        _rows = rows;
        _blocks = new Block[columns, rows];

        ServiceProvider.BoardRenderer.AdjustBoard(columns, rows, _blocks);
    }

    private void InitializeBlocks()
    {
        foreach (var block in _blocks)
        {
            block?.InitBlock();
        }
    }

    public void Reset()
    {
        _availableTriangles.Clear();
    }

    public Block GetRightBlock(int x, int y)
    {
        if( x+1 >= _columns) return null;

        return _blocks[x+1,y];
    }

    public Block GetLeftBlock(int x, int y)
    {
        if( x-1 < 0) return null;

        return _blocks[x-1,y];
    }

    public Block GetUpperBlock(int x, int y)
    {
        if( y+1 >= _rows) return null;

        return _blocks[x,y+1];
    }

    public Block GetBellowBlock(int x, int y)
    {
        if( y-1 < 0) return null;

        return _blocks[x,y-1];
    }

}
