
public class Block
{
    private readonly int X;
    private readonly int Y;

    public readonly Triangle upperTriangle;
    public readonly Triangle bellowTriangle;
    public readonly Triangle rightTriangle;
    public readonly Triangle leftTriangle;

    public Block(int x, int y)
    {
        this.X = x;
        this.Y = y;

        upperTriangle = new Triangle(X,Y);
        bellowTriangle = new Triangle(X,Y);
        rightTriangle = new Triangle(X,Y);
        leftTriangle = new Triangle(X,Y);
    }

    public void InitBlock()
    {
        upperTriangle.Init(leftTriangle, rightTriangle, Grid.Instance.GetUpperBlock(X,Y).bellowTriangle );
        bellowTriangle.Init(leftTriangle, rightTriangle, Grid.Instance.GetBellowBlock(X,Y).upperTriangle );
        rightTriangle.Init(upperTriangle, bellowTriangle, Grid.Instance.GetLeftBlock(X,Y).rightTriangle );
        leftTriangle.Init(upperTriangle, bellowTriangle, Grid.Instance.GetRightBlock(X,Y).leftTriangle );
    }

}