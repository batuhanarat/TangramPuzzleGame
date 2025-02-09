using System.Linq;

public class Triangle
{
    private Triangle[] _neighbors;
    private readonly int X;
    private readonly int Y;
    private bool isOccupied = false;

    public Triangle(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public void Init(Triangle tri1, Triangle tri2, Triangle tri3)
    {
        if(tri3 != null )
        {
            _neighbors.Append(tri3);
        }
        _neighbors.Append(tri1);
        _neighbors.Append(tri2);
    }


}