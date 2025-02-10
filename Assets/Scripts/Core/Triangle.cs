using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    [SerializeField] private TriangleType TriangleType;
    [SerializeField] private SpriteRenderer TriangleRenderer;

    private List<Triangle> _neighbors = new();
    private int X;
    private int Y;
    private bool _isOccupied = false;

    public bool IsAvailableToCapture  => !_isOccupied ;

    public void Init(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    public void ChangeColor(Color color)
    {
        _isOccupied = true;
        TriangleRenderer.color = color;
    }

    public void SetNeighbors(Triangle tri1, Triangle tri2, Triangle tri3)
    {
        if(tri3 != null )
        {
            _neighbors.Add(tri3);
        }
        _neighbors.Add(tri1);
        _neighbors.Add(tri2);
    }

    public List<Triangle> GetAllNeighbors()
    {
        return _neighbors;
    }


}