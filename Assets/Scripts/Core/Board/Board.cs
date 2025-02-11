using System.Collections.Generic;

public class Board
{

    #region Private Variables

        private static Board _instance;
        private Block[,] blocks;
        private int _columns;
        private int _rows;
        private readonly List<Triangle> availableTriangles = new();

    #endregion



    #region Singleton

        public static Board Instance {
            get
        {
                if (_instance == null)
                {
                    _instance = new Board();
                }
                return _instance;
            }
        }

        private Board(){}

    #endregion
    public void AddToAvailableTriangles(Triangle triangle)
    {
        availableTriangles.Add(triangle);
    }

    public void RemoveFromAAvailableTriangels(Triangle triangle)
    {
        availableTriangles.Remove(triangle);
    }

    public Triangle GetRandomTriangle()
    {
        var randomIndex = UnityEngine.Random.Range(0,availableTriangles.Count);
        return availableTriangles[randomIndex];
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
        blocks = new Block[columns, rows];

        BoardRenderer.Instance.AdjustBoard(columns, rows, blocks);
    }

    private void InitializeBlocks()
    {
        foreach (var block in blocks)
        {
            block?.InitBlock();
        }
    }


    public void Reset()
    {
        availableTriangles.Clear();
    }


    public Block GetRightBlock(int x, int y)
    {
        if( x+1 >= _columns) return null;

        return blocks[x+1,y];
    }

    public Block GetLeftBlock(int x, int y)
    {
        if( x-1 < 0) return null;

        return blocks[x-1,y];
    }

    public Block GetUpperBlock(int x, int y)
    {
        if( y+1 >= _rows) return null;

        return blocks[x,y+1];
    }

    public Block GetBellowBlock(int x, int y)
    {
        if( y-1 < 0) return null;

        return blocks[x,y-1];
    }

}
