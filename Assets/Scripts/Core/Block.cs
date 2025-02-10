using UnityEngine;

public class Block
{
    private readonly int X;
    private readonly int Y;
    public UnityEngine.Vector3 Position;
    public Vector2Int Coordinates { get => new Vector2Int(X,Y); }

    public Triangle UpperTriangle { get; private set; }
    public Triangle LowerTriangle { get; private set; }
    public Triangle RightTriangle { get; private set; }
    public Triangle LeftTriangle { get; private set; }

    public bool IsLeftTriangleOccuppied { get; private set; }
    public bool IsRightTriangleOccuppied { get; private set; }
    public bool IsUpperTriangleOccuppied { get; private set; }
    public bool IsLowerTriangleOccuppied { get; private set; }


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
    public void RemoveFromBlock(Triangle triangle)
    {
        switch(triangle.Type)
        {
            case TriangleType.LEFT:
                    IsLeftTriangleOccuppied = false;
                    break;

            case TriangleType.RIGHT:
                    IsRightTriangleOccuppied = false;
                    break;

            case TriangleType.UP:
                    IsUpperTriangleOccuppied = false;
                    break;

            case TriangleType.DOWN:
                    IsLowerTriangleOccuppied = false;
                    break;

            default:
            break;
        }

    }

    public bool TryAddToBlock(Triangle triangle)
    {
        switch(triangle.Type)
        {
            case TriangleType.LEFT:

                if(!IsLeftTriangleOccuppied){

                    IsLeftTriangleOccuppied = true;
                    return true;

                } else {
                    return false;
                }

            case TriangleType.RIGHT:

                if(!IsRightTriangleOccuppied){

                        IsRightTriangleOccuppied = true;
                        return true;

                    } else {
                        return false;
                    }

            case TriangleType.UP:

                if(!IsUpperTriangleOccuppied){

                    IsUpperTriangleOccuppied = true;
                    return true;

                } else {
                    return false;
                }

            case TriangleType.DOWN:

                if(!IsLowerTriangleOccuppied){

                    IsLowerTriangleOccuppied = true;
                    return true;

                } else {
                    return false;
                }

            default:
            break;
        }
        return false;

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