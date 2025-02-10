using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board
{
    [SerializeField] private SpriteRenderer GridRenderer;
    public int OnDraggedSortingOrder { get =>  activePieces.Count + 1; }
    public int InPlacedSortingOrder { get =>  -1 ;}

    private List<Vector3> SpawnPositions = new List<Vector3>
    {
        new Vector3(0.615835726f, -3.57771254f, 0f),
        new Vector3(-2.17008781f, -5.54252195f, 0f),
        new Vector3(1.73020518f, -6.26099682f, 0f),
        new Vector3(1.9941349f, -3.84164238f, 0f)
    };


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
    public Block[,] GetBlocks()
    {
        return blocks;
    }

    private Block[,] blocks;
    private int _columns;
    private int _rows;
    public List<Triangle> availableTriangles = new();
    private readonly Color[] AllColors = {
        new Color(0xE2/255f, 0x43/255f, 0x43/255f), // E24343 - Kırmızımsı
        new Color(0xC2/255f, 0xFF/255f, 0xB3/255f), // C2FFB3 - Açık yeşil
        new Color(0x43/255f, 0xD2/255f, 0xE2/255f), // 43D2E2 - Açık mavi
        new Color(0xE2/255f, 0xE2/255f, 0x43/255f), // E2E243 - Sarı
        new Color(0x98/255f, 0x43/255f, 0xE2/255f), // 9843E2 - Mor
        new Color(0xE2/255f, 0x8D/255f, 0x43/255f)  // E28D43 - Turuncu
    };
    private Color[] _choosenColors;
    private List<Piece> activePieces = new();


    private Board()
    {
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

        for (int i = 0; i < pieceCount; i++)
        {
            _choosenColors[i] = AllColors[i];

            GameObject pieceGO = new("Piece");
            var piece = pieceGO.AddComponent<Piece>();

            Triangle triangle = GetRandomTriangle();
            piece.Init(triangle, _choosenColors[i]);
            activePieces.Add(piece);
        }
        List<Piece> pieces = activePieces.ToList();

        bool isCreating = true;
        while (isCreating)
        {
            for (int i = pieces.Count - 1; i >= 0; i--)
            {
                if (!pieces[i].TryProgress())
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
        Debug.Log("pieces " + activePieces.Count);
        ArrangeSortingOrders();
        MovePiecesFromBoard();

    }


    private void MovePiecesFromBoard()
    {
        for(int i = 0 ; i < activePieces.Count ; i++)
        {
            activePieces[i].transform.position = SpawnPositions[i];
        }
    }

    private void ArrangeSortingOrders()
    {
        for (int i = 0; i < activePieces.Count; i++)
        {
            Debug.Log("setting sorting order for piece" + i);
            activePieces[i].SetSortingOrder(i);
        }
    }
}
