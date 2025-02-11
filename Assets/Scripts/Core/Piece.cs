using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    #region Private Variables

        private Color pieceColor;
        private List<Triangle> unitTriangles;
        private List<Triangle> moveableTriangles = new();
        private int _sortingOrder;
        private PieceManager pieceManager;
        private Board board;
        private bool _isLocked;
        private bool _isPlaced;

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
        if (_isPlaced)
        {
            RemoveFromBoard();
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
        }

        TryProgress();
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
            int index = UnityEngine.Random.Range(0, moveableTriangles.Count);
            var triangletoCapture = moveableTriangles[index];

            unitTriangles.Add(triangletoCapture);
            triangletoCapture.ChangeColor(pieceColor,this);
            board.RemoveFromAvailableTriangels(triangletoCapture);
            if (triangletoCapture.transform != null && this.transform != null)
            {
                triangletoCapture.transform.parent = this.transform;
            }
                return true;
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
    }

    private IEnumerator CaptureCoroutine(Action<bool> callback)
    {
        yield return new WaitForSeconds(1f);
        int index = UnityEngine.Random.Range(0, moveableTriangles.Count);
        var triangletoCapture = moveableTriangles[index];

        unitTriangles.Add(triangletoCapture);
        triangletoCapture.ChangeColor(pieceColor,this);
        board.RemoveFromAvailableTriangels(triangletoCapture);
        callback(true);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

}