using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Piece : MonoBehaviour
{
    #region Private Variables

        private Color pieceColor;
        private List<Triangle> unitTriangles;
        private List<Triangle> moveableTriangles = new();
        private int _sortingOrder;
        private IPieceManager pieceManager;
        private Board board;
        private bool _isLocked;
        public Vector3 InitialPositionOffset;


    #endregion

    #region Properties

        public bool CanBeDragged  => !_isLocked;
        public Vector3 Position { get => transform.position; }
        public bool IsOnTheBoard = false;

    #endregion

    private void Awake()
    {
        board = ServiceProvider.Board;
        pieceManager = ServiceProvider.PieceManager;
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
        foreach (var triangle in unitTriangles)
        {
            triangle.SetSortingOrder(pieceManager.OnDraggedSortingOrder);
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
        this.pieceColor = color;
        unitTriangles = new()
        {
            firstTriangle
        };
        firstTriangle.ChangeColor(color,this);
        board.RemoveFromAvailableTriangels(firstTriangle);

        if (firstTriangle.transform != null && this.transform != null)
        {
            firstTriangle.transform.parent = this.transform;
            InitialPositionOffset = firstTriangle.transform.localPosition;
        }

        TryProgress();

       //CoroutineRunner.Instance.StartCoroutine(TryProgressWithAnimation());
    }


    private void RemoveFromBoard()
    {
        foreach(var unit in unitTriangles)
        {
            unit.ExtractFromBoard();
        }
        IsOnTheBoard = false;
        pieceManager.OnPieceRemoved();
    }


    private bool TryPlace()
    {
        foreach(var triangle in unitTriangles)
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
        foreach(var triangle in unitTriangles)
        {
            triangle.PlaceToBlock();
            triangle.SetSortingOrder(pieceManager.InPlacedSortingOrder);
        }

        IsOnTheBoard = true;
        PlayPlacedAnimation();
        pieceManager.OnPiecePlaced();
    }


    private void OnPlacedFailed()
    {
        foreach(var triangle in unitTriangles)
        {
            triangle.SetInitialSortingOrder();
        }

        IsOnTheBoard = false;
    }

    private void InitMovableTriangles()
    {

        foreach( var triangle in unitTriangles)
        {
            var neighbors = triangle.GetAllNeighbors();
            foreach(var neighbor in neighbors)
            {
                if(neighbor.IsAvailableToCapture)
                {
                    moveableTriangles.Add(neighbor);
                }
            }
        }
    }

    public bool TryProgress()
    {
        moveableTriangles.Clear();
        InitMovableTriangles();

        if (moveableTriangles.Count == 0)
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

        moveableTriangles.Clear();
        InitMovableTriangles();

        if (moveableTriangles.Count == 0)
        {
            yield  break;
        }
        yield return new WaitForSeconds(0.05f);
        Capture();
        yield return true;
    }

    private void Capture()
    {
        int index = UnityEngine.Random.Range(0, moveableTriangles.Count);
        var triangletoCapture = moveableTriangles[index];

        unitTriangles.Add(triangletoCapture);
        triangletoCapture.ChangeColor(pieceColor,this);
        board.RemoveFromAvailableTriangels(triangletoCapture);
        if (triangletoCapture.transform != null && this.transform != null)
        {
            triangletoCapture.transform.parent = this.transform;
        }
    }


    public void SetSortingOrder(int order)
    {
        _sortingOrder = order;
        foreach(var unit in unitTriangles)
        {
            unit.SetSortingOrder(order);
            unit.InitialSortingOrder = order;
        }
        transform.position = transform.position - new Vector3(0,0,order);
    }

    public void PlayPlacedAnimation()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var triangle in unitTriangles)
        {
            var spriteRenderer = triangle.GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

            sequence.Join(
                spriteRenderer.DOColor(transparentColor, 0.1f)
                    .SetEase(Ease.InOutQuad)
                    .SetLoops(2, LoopType.Yoyo)
            );
        }
    }


    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }


}