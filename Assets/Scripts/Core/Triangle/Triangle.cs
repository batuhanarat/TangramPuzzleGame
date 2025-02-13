using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    #region Private Variables

        private List<Triangle> _neighbors = new();
        private Vector2Int _coordinates;
        private Block _blockToPlace;
        private bool _isOccupied = false;
        private Piece _piece;

    #endregion

    #region Serialized Variables

        [SerializeField] private TriangleType triangleType;
        [SerializeField] private SpriteRenderer TriangleRenderer;

    #endregion

    #region Properties

        public TriangleType TriangleType { get => triangleType; }
        public int InitialSortingOrder { get; set; }
        public bool IsAvailableToCapture  => !_isOccupied ;

    #endregion

    public void Init(int x, int y)
    {
        _coordinates = new Vector2Int(x,y);
    }

    public void ExtractFromBoard()
    {
        _blockToPlace.RemoveFromBlock(this);
    }

    public void SetInitialSortingOrder()
    {
        SetSortingOrder(InitialSortingOrder);
    }

    public bool CanPlace()
    {
        if(!ServiceProvider.BoardRenderer.GetBlockFromPosition(transform.position, out Block block))
        {
            return false ;
        }

        if (block.CheckCanAddToBlock(this) )
        {
            _blockToPlace = block;

            return true;
        }

        return false;
    }

    public void PlaceToBlock()
    {
        if(_blockToPlace != null)
        {
            transform.position = _blockToPlace.Position;
            _blockToPlace.OccupyTriangleInBlock(this);
        }
    }

    public void OccupyTriangle(Color color, Piece piece)
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

    public Piece GetPiece()
    {
        return _piece;
    }


}