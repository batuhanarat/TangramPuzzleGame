using System;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    [SerializeField] private SpriteRenderer GridRenderer;
    public static Board Instance;
    private Block[,] blocks;
    private int _columns;
    private int _rows;
    public List<Triangle> availableTriangles = new();
    private readonly Color[] AllColors = {Color.blue, Color.green, Color.red, Color.magenta, Color.yellow, Color.grey};
    private Color[] _choosenColors;

    public Board(int col, int row, int seed, int pieceCount)
    {
        Init(col,row);
        CreateTangram(seed,pieceCount);
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

        for(int col = 0; col < columns ; col++ )
        {
            for(int row = 0 ; row < rows ; row++)
            {
                var block = new Block(col,row);
                blocks[col,row] = block;
            }
        }

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

        ShuffleArray(AllColors);


        for(int i = 0 ; i < pieceCount ; i++) {
            _choosenColors[i] = AllColors[i];

            GameObject pieceGO = new("Piece");
            var piece =  pieceGO.AddComponent<Piece>();

            Triangle triangle = GetRandomTriangle();
            piece.Init(triangle, _choosenColors[i]);
        }

        /* Dictionary<Color , int> keyValuePairs = new();


        Dictionary<Color, string> colorNames = new Dictionary<Color, string>
        {
            { Color.blue, "Blue" },
            { Color.green, "Green" },
            { Color.red, "Red" },
            { Color.magenta, "Magenta" },
            { Color.yellow, "Yellow" },
            { Color.grey, "Grey" }
        };


        for(int i = 0 ; i < 100 ; i++)
        {
            int colorIndex = Random.Range(0,5);
            if(!keyValuePairs.ContainsKey(colors[colorIndex]) )  {
                keyValuePairs.Add(colors[colorIndex],1);
            } else {
                keyValuePairs[colors[colorIndex]]++;
            }
        }

        foreach(var element in keyValuePairs) {
            Debug.Log(colorNames[element.Key] +" : " + element.Value);
        }
        */

    }


}