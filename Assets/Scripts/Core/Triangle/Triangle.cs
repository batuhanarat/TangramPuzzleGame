using System.Collections.Generic;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    #region Private Variables

        public TriangleType TriangleType { get => triangleType; }
        private List<Triangle> _neighbors = new();
        private Vector2Int coordinates;
        private Block blockToPlace;
        private bool _isOccupied = false;

    #endregion

    #region Serialized Variables

        [SerializeField] private TriangleType triangleType;
        [SerializeField] private SpriteRenderer TriangleRenderer;
        [SerializeField] private Piece _piece;

    #endregion

    #region Properties

        public int InitialSortingOrder { get; set; }
        public bool IsAvailableToCapture  => !_isOccupied ;

    #endregion

    public void Init(int x, int y)
    {
        coordinates = new Vector2Int(x,y);
    }

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
        if(!ServiceProvider.BoardRenderer.GetBlockFromPosition(transform.position, out Block block))
        {
            return false ;
        }

        if (block.CheckCanAddToBlock(this) )
        {
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

    public Piece GetPiece()
    {
        return _piece;
    }


}