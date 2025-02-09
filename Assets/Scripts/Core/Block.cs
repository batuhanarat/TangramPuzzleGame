
using System.Numerics;

public class Block
{
    private readonly int X;
    private readonly int Y;
    private readonly Vector3 position;

    public readonly Triangle upperTriangle;
    public readonly Triangle bellowTriangle;
    public readonly Triangle rightTriangle;
    public readonly Triangle leftTriangle;

    public Block(int x, int y)
    {
        this.X = x;
        this.Y = y;

        upperTriangle = TriangleFactory.Instance.GetTriangleFromType(TriangleType.UP);
        upperTriangle.Init(x,y);
        Board.Instance.availableTriangles.Add(upperTriangle);

        bellowTriangle = TriangleFactory.Instance.GetTriangleFromType(TriangleType.DOWN);
        bellowTriangle.Init(x,y);
        Board.Instance.availableTriangles.Add(bellowTriangle);

        rightTriangle = TriangleFactory.Instance.GetTriangleFromType(TriangleType.RIGHT);
        rightTriangle.Init(x,y);
        Board.Instance.availableTriangles.Add(rightTriangle);

        leftTriangle = TriangleFactory.Instance.GetTriangleFromType(TriangleType.LEFT);
        leftTriangle.Init(x,y);
        Board.Instance.availableTriangles.Add(leftTriangle);

    }

    public void InitBlock()
    {
        Triangle bellowTriangle1;
        if(Board.Instance.GetUpperBlock(X,Y) != null)
        {
            bellowTriangle1 =  Board.Instance.GetUpperBlock(X,Y).bellowTriangle;
        } else {
            bellowTriangle1  = null;
        }
        upperTriangle.SetNeighbors(leftTriangle, rightTriangle, bellowTriangle1 );





        Triangle upperTriangle1;
        if(Board.Instance.GetBellowBlock(X,Y) != null)
        {
            upperTriangle1 =  Board.Instance.GetBellowBlock(X,Y).upperTriangle;
        } else {
            upperTriangle1  = null;
        }

        bellowTriangle.SetNeighbors(leftTriangle, rightTriangle, upperTriangle1 );




        Triangle rightTriangle1;
        if(Board.Instance.GetLeftBlock(X,Y) != null)
        {
            rightTriangle1 =   Board.Instance.GetLeftBlock(X,Y).rightTriangle;
        } else {
            rightTriangle1  = null;
        }
        rightTriangle.SetNeighbors(upperTriangle, bellowTriangle, rightTriangle1 );




        Triangle leftTriangle1;
        if(Board.Instance.GetRightBlock(X,Y) != null)
        {
            leftTriangle1 =  Board.Instance.GetRightBlock(X,Y).leftTriangle ;
        } else {
            leftTriangle1  = null;
        }
        leftTriangle.SetNeighbors(upperTriangle, bellowTriangle, leftTriangle1);



    }


}