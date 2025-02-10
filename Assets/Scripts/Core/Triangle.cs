using System;
using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    [SerializeField] private TriangleType TriangleType;

    public TriangleType Type { get => TriangleType; }
    [SerializeField] private SpriteRenderer TriangleRenderer;

    private List<Triangle> _neighbors = new();
    private int X;
    private int Y;
    private bool _isOccupied = false;
    public int InitialSortingOrder;
    private Vector3 _offset;
    private Vector3 mousePositionOnInputFinishedWithOffset;
    private Vector3 mousePositionOnInputFinished;
    private Vector3 mousePosition ;
    private bool _isDragging = false;
    private Block blockToPlace;
    [SerializeField] private Piece _piece;

    public bool IsAvailableToCapture  => !_isOccupied ;


    public void OnMouseDown()
    {
        if (_piece == null) return;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        _offset = _piece.transform.position - mousePosition;

        _isDragging = true;

    }
    public void OnMouseDrag()
    {
        if (_piece == null || !_isDragging) return;

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        _piece.SetPosition(mousePosition + _offset);

    }
    public void OnMouseUp()
    {
        _isDragging = false;
        mousePositionOnInputFinishedWithOffset = mousePosition + _offset;
        mousePositionOnInputFinished = mousePosition;

        if(_piece.IsOnTheBoard)
        {
            _piece.RemoveFromBoard();
        }

        if(_piece.TryPlace()) {
            SetSortingOrder(Board.Instance.InPlacedSortingOrder);
        } else{
            SetSortingOrder(InitialSortingOrder);
        }
    }
    public void ExtractFromBoard()
    {
        blockToPlace.RemoveFromBlock(this);
    }


    public bool CanPlace()
    {

        if(!BoardRenderer.Instance.GetBlockFromPosition(transform.position, out Block block))
        {
            Debug.Log(Type + "could not find any block in CanPlace function ");
            return false ;

        }

        if (block.TryAddToBlock(this) ) {
        blockToPlace = block;

        return true;
        }

        return false;

    }

    public void ExtractFromBlock()
    {

    }

    public void PlaceToBlock()
    {
        if(blockToPlace != null)
        {
            transform.position = blockToPlace.Position;
            Debug.Log("PLACED");
        }
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