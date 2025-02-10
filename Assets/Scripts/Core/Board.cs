using System;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    [SerializeField] private SpriteRenderer GridRenderer;
    private static Board _instance;

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



    private Block[,] blocks;
    private int _columns;
    private int _rows;
    public List<Triangle> availableTriangles = new();
    private readonly Color[] AllColors = {Color.blue, Color.green, Color.red, Color.magenta, Color.yellow, Color.grey};
    private Color[] _choosenColors;
    private List<Piece> pieces = new();



    private Board()
    {
       // Init(col,row);
        //CreateTangram(seed,pieceCount);
    }

    public Triangle GetRandomTriangle()
    {
        var randomIndex = UnityEngine.Random.Range(0,availableTriangles.Count);
        return availableTriangles[randomIndex];
    }

    public void Init(int columns, int rows)
    {
        _columns = columns;
        _rows = rows;
        blocks = new Block[columns,rows];

        BoardRenderer.Instance.AdjustBoard(columns, rows, blocks);

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

    //refactor to somewhere else probably to extension method
    public void ShuffleArray(Array array)
    {
        for(int i = array.Length -1 ; i< array.Length ; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            object temp = array.GetValue(i);
            array.SetValue(array.GetValue(randomIndex), i);
            array.SetValue(temp, randomIndex);
        }
    }

    public void CreateTangram(int seed, int pieceCount)
    {
        UnityEngine.Random.InitState(seed);
        _choosenColors = new Color[pieceCount];

        //ShuffleArray(AllColors);

        Debug.Log("müsait üçgen sayısı: " + availableTriangles.Count);


        for(int i = 0 ; i < pieceCount ; i++) {
            _choosenColors[i] = AllColors[i];

            GameObject pieceGO = new("Piece");
            var piece = pieceGO.AddComponent<Piece>();

            Triangle triangle = GetRandomTriangle();
            piece.Init(triangle, _choosenColors[i]);
            pieces.Add(piece);
        }



        bool isCreating = true;
        while (isCreating)
        {
            for (int i = pieces.Count - 1; i >= 0; i--)
            {
                if(!pieces[i].TryProgress())
                {
                        pieces.RemoveAt(i);

                        if (pieces.Count == 0)
                        {
                            isCreating = false;
                            Debug.Log("All pieces are created");
                            break;
                        }
                }
            }
        }
    }

    }
