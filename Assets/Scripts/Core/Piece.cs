using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Piece : MonoBehaviour
{
    #region Private Variables

        private readonly List<Triangle> _unitTriangles = new();
        private readonly List<Triangle> _moveableTriangles = new();
        private int _sortingOrder;
        private Color _pieceColor;
        private bool _isLocked;
        private bool _isAnimationPlaying;
        private IPieceManager _pieceManager;
        private Board _board;
        private static readonly float WAIT_DURATION_AFTER_CAPTURE = 0.1f;
        private static readonly float BLINK_ANIMATION_DURATION = 0.1f;

    #endregion

    #region Properties

        public bool CanBeDragged  => !_isLocked;
        public Vector3 Position { get => transform.position; }
        public bool IsOnTheBoard = false;
        public Vector3 InitialPositionOffset { get; private set; }


    #endregion

    private void Awake()
    {
        _board = ServiceProvider.Board;
        _pieceManager = ServiceProvider.PieceManager;
    }


    public void OnStartDrag()
    {
        if (IsOnTheBoard)
        {
            RemoveFromBoard();
        }

        MoveToTop();
    }

    private void MoveToTop()
    {
        foreach (var triangle in _unitTriangles)
        {
            triangle.SetSortingOrder(_pieceManager.OnDraggedSortingOrder);
        }
    }

    public void OnEndDrag()
    {
        if (TryPlace())
        {
            OnPlacedSuccessful();
        }
        else
        {
            OnPlacedFailed();
        }
    }


    public void Init(Triangle firstTriangle, Color color)
    {
        this._pieceColor = color;
        _unitTriangles.Add(firstTriangle);
        firstTriangle.OccupyTriangle(color,this);
        _board.RemoveFromAvailableTriangels(firstTriangle);

        if (firstTriangle.transform != null && this.transform != null)
        {
            firstTriangle.transform.parent = this.transform;
            InitialPositionOffset = firstTriangle.transform.localPosition;
        }

        TryProgress();

    }


    private void RemoveFromBoard()
    {
        foreach(var unit in _unitTriangles)
        {
            unit.ExtractFromBoard();
        }
        IsOnTheBoard = false;
        _pieceManager.OnPieceRemoved();
    }

    private bool TryPlace()
    {
        foreach(var triangle in _unitTriangles)
        {
            if(!triangle.CanPlace())
            {
                return false;
            }
        }
        return true;
    }

    private void OnPlacedSuccessful()
    {
        foreach(var triangle in _unitTriangles)
        {
            triangle.PlaceToBlock();
            triangle.SetSortingOrder(_pieceManager.InPlacedSortingOrder);
        }

        IsOnTheBoard = true;
        PlayPlacedAnimation();
        _pieceManager.OnPiecePlaced();
    }


    private void OnPlacedFailed()
    {
        foreach(var triangle in _unitTriangles)
        {
            triangle.SetInitialSortingOrder();
        }

        IsOnTheBoard = false;
    }

    private void InitMovableTriangles()
    {

        foreach( var triangle in _unitTriangles)
        {
            var neighbors = triangle.GetAllNeighbors();
            foreach(var neighbor in neighbors)
            {
                if(neighbor.IsAvailableToCapture)
                {
                    _moveableTriangles.Add(neighbor);
                }
            }
        }
    }

    public bool TryProgress()
    {
        _moveableTriangles.Clear();
        InitMovableTriangles();

        if (_moveableTriangles.Count == 0)
        {
            return false;
        }
        else
        {
            Capture();
            return true;
        }
    }
    public IEnumerator TryProgressWithAnimation()
    {

        _moveableTriangles.Clear();
        InitMovableTriangles();

        if (_moveableTriangles.Count == 0)
        {
            yield  break;
        }
        yield return new WaitForSeconds(WAIT_DURATION_AFTER_CAPTURE);
        Capture();
        yield return true;
    }

    private void Capture()
    {
        int index = UnityEngine.Random.Range(0, _moveableTriangles.Count);
        var triangletoCapture = _moveableTriangles[index];

        _unitTriangles.Add(triangletoCapture);
        triangletoCapture.OccupyTriangle(_pieceColor,this);
        _board.RemoveFromAvailableTriangels(triangletoCapture);

        if (triangletoCapture.transform != null && this.transform != null)
        {
            triangletoCapture.transform.parent = this.transform;
        }
    }

    public void SetSortingOrder(int order)
    {
        _sortingOrder = order;
        foreach(var unit in _unitTriangles)
        {
            unit.SetSortingOrder(order);
            unit.InitialSortingOrder = order;
        }
        transform.position = transform.position - new Vector3(0,0,order);
    }

    public void PlayPlacedAnimation()
    {
        if (_isAnimationPlaying) return;

        _isAnimationPlaying = true;
        Sequence sequence = DOTween.Sequence();

        foreach (var triangle in _unitTriangles)
        {
            var spriteRenderer = triangle.GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            Color transparentColor = new(originalColor.r, originalColor.g, originalColor.b, 0f);

            sequence.Join(
                spriteRenderer.DOColor(transparentColor, BLINK_ANIMATION_DURATION)
                    .SetEase(Ease.InOutQuad)
                    .SetLoops(2, LoopType.Yoyo)
            );
        }

        sequence.OnComplete(() => _isAnimationPlaying = false);
    }


    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }


}