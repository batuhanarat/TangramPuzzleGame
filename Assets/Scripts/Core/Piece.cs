using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private Color pieceColor;
    private List<Triangle> unitTriangles;
    private List<Triangle> moveableTriangles = new();
    private int _sortingOrder;
    public Vector3 Position { get => transform.position; }

    public void OnMouseDown()
    {
                Debug.Log("BU PİECE E TIKLANDI ");
        var coord = unitTriangles[0].GetCoord();
        Debug.Log("BU PİECE E TIKLANDI " + coord.x +" :" + coord.y);
    }
    public void Init(Triangle firstTriangle, Color color)
    {
        this.pieceColor = color;
        unitTriangles = new()
        {
            firstTriangle
        };
        firstTriangle.ChangeColor(color,this);
        Board.Instance.availableTriangles.Remove(firstTriangle);

        if (firstTriangle.transform != null && this.transform != null)
        {
            firstTriangle.transform.parent = this.transform;
        }

        TryProgress();
    }

    public bool TryPlace()
    {
        foreach(var triangle in unitTriangles)
        {

            // Her bir triangle için,
            // pozisyonu hangi bloğa denk geliyor ona bakmalı
            // eğer bulunduğu bloktaki o typeda başka bir triangle yok ise yani occupied değilse -> testi geçti
            // true döndür  ->
            // eğer herhangi biri false döndürürse koymayı iptal et

            if(!triangle.CanPlace())
            {
                return false;
            }


        }

        //notifyalltrianglestoplaced();
        foreach(var triangle in unitTriangles) {
            triangle.PlaceToBlock();
        }
        return true;

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
            Board.Instance.availableTriangles.Remove(triangletoCapture);
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
        Board.Instance.availableTriangles.Remove(triangletoCapture);
        callback(true);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }




}