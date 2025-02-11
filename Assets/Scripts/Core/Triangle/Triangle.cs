using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    [SerializeField] private TriangleType triangleType;
    [SerializeField] private SpriteRenderer TriangleRenderer;


    public TriangleType TriangleType { get => triangleType; }

    private List<Triangle> _neighbors = new();
    private Vector2Int coordinates;

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

/*
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
            _piece.OnPlacedSuccessful();
        } else{
            _piece.OnPlacedFailed();
        }
    }
    */
    public void ExtractFromBoard()
    {
        blockToPlace.RemoveFromBlock(this);
    }

    public void SetInitialSortingOrder()
    {
        SetSortingOrder(InitialSortingOrder);
    }

    public bool CanPlace()
    {

        if(!BoardRenderer.Instance.GetBlockFromPosition(transform.position, out Block block))
        {
            return false ;

        }

        if (block.CheckCanAddToBlock(this) ) {

            blockToPlace = block;

            return true;
        }

        return false;

    }

    public void PlaceToBlock()
    {
        if(blockToPlace != null)
        {
            transform.position = blockToPlace.Position;
            blockToPlace.OccupyTriangleInBlock(this);
        }
    }

    public void Init(int x, int y)
    {
        coordinates = new Vector2Int(x,y);
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