using UnityEngine;

public class Block
{
    private readonly int X;
    private readonly int Y;
    private readonly UnityEngine.Vector3 Position;

    public Triangle UpperTriangle { get; private set; }
    public Triangle LowerTriangle { get; private set; }
    public Triangle RightTriangle { get; private set; }
    public Triangle LeftTriangle { get; private set; }

    public Block(int x, int y, UnityEngine.Vector3 boardCenter, float cellSize, int gridSize)
    {
       // gridSize burada rows ya da col, -> çünkü kare
        this.X = x;
        this.Y = y;

        Position = boardCenter + new UnityEngine.Vector3(
            (x * cellSize) - (cellSize * (gridSize - 1) / 2f),
            (y * cellSize) - (cellSize * (gridSize - 1) / 2f),
            0f
        );

        UpperTriangle = CreateTriangle(TriangleType.UP, Position, cellSize);
        LowerTriangle = CreateTriangle(TriangleType.DOWN, Position, cellSize);
        RightTriangle = CreateTriangle(TriangleType.RIGHT, Position, cellSize);
        LeftTriangle = CreateTriangle(TriangleType.LEFT, Position, cellSize);

    }


    private Triangle CreateTriangle(TriangleType type, UnityEngine.Vector3 position, float cellSize)
    {
        Triangle triangle = TriangleFactory.Instance.GetTriangleFromType(type, position);

        SpriteRenderer sr = triangle.GetComponent<SpriteRenderer>();
        float scale = cellSize / sr.sprite.bounds.size.x;
        triangle.transform.localScale = new UnityEngine.Vector3(scale, scale, 1f);

        triangle.Init(X, Y);
        Board.Instance.availableTriangles.Add(triangle);
        return triangle;
    }

    public void InitBlock()
    {
        Triangle bellowTriangle1;
        if(Board.Instance.GetUpperBlock(X,Y) != null)
        {
            bellowTriangle1 =  Board.Instance.GetUpperBlock(X,Y).LowerTriangle;
        } else {
            bellowTriangle1  = null;
        }
        UpperTriangle.SetNeighbors(LeftTriangle, RightTriangle, bellowTriangle1 );





        Triangle upperTriangle1;
        if(Board.Instance.GetBellowBlock(X,Y) != null)
        {
            upperTriangle1 =  Board.Instance.GetBellowBlock(X,Y).UpperTriangle;
        } else {
            upperTriangle1  = null;
        }

        LowerTriangle.SetNeighbors(LeftTriangle, RightTriangle, upperTriangle1 );




        Triangle leftTriangle1;
        if(Board.Instance.GetRightBlock(X,Y) != null)
        {
            leftTriangle1 =   Board.Instance.GetRightBlock(X,Y).LeftTriangle;
        } else {
            leftTriangle1  = null;
        }
        RightTriangle.SetNeighbors(UpperTriangle, LowerTriangle, leftTriangle1 );




        Triangle rightTriangle1;
        if(Board.Instance.GetLeftBlock(X,Y) != null)
        {
            rightTriangle1 =  Board.Instance.GetLeftBlock(X,Y).RightTriangle ;
        } else {
            rightTriangle1  = null;
        }
        LeftTriangle.SetNeighbors(UpperTriangle, LowerTriangle, rightTriangle1);



    }


}