
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid Instance;
    private  Block[,] blocks;
    private int _columns;
    private int _rows;

    void Awake()
    {
        Instance = this;
    }

    public void Init(int columns, int rows)
    {
        _columns = columns;
        _rows = rows;
        blocks = new Block[columns,rows];
        foreach(var block in blocks)
        {
            block.InitBlock();
        }
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

    public void CreateTangram(int seed, int pieceCount)
    {

    }


}