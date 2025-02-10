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
    public int sortingOrder;
    private Vector3 _offset;
    private bool _isDragging = false;
   [SerializeField] private Piece _piece;

    public bool IsAvailableToCapture  => !_isOccupied ;


    public void OnMouseDown()
    {
        if (_piece == null) return;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        _offset = _piece.transform.position - mousePosition;

        _isDragging = true;

    }
    public void OnMouseDrag()
    {
        if (_piece == null || !_isDragging) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        _piece.SetPosition(mousePosition + _offset);

    }
    public void OnMouseUp()
    {
        _isDragging = false;
    }

    public void Init(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    public Vector2Int GetCoord()
    {
        return new Vector2Int(X,Y);
    }
    public void ChangeColor(Color color, Piece piece)
    {
        _isOccupied = true;
        _piece = piece;
        TriangleRenderer.color = color;
    }

    public void SetSortingOrder(int order)
    {
        sortingOrder = order;
        TriangleRenderer.sortingOrder = order;
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