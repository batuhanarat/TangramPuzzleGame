using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private Color pieceColor;
    private List<Triangle> unitTriangles;
    private List<Triangle> moveableTriangles = new();


    public void Init(Triangle firstTriangle, Color color)
    {
        this.pieceColor = color;
        unitTriangles = new()
        {
            firstTriangle
        };
        firstTriangle.ChangeColor(color);
        Board.Instance.availableTriangles.Remove(firstTriangle);

        if (firstTriangle.transform != null && this.transform != null)
        {
            firstTriangle.transform.parent = this.transform;
        }

        TryProgress();
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
            triangletoCapture.ChangeColor(pieceColor);
            Board.Instance.availableTriangles.Remove(triangletoCapture);
            if (triangletoCapture.transform != null && this.transform != null)
            {
                triangletoCapture.transform.parent = this.transform;
            }
                return true;
            }
    }

    private IEnumerator CaptureCoroutine(Action<bool> callback)
    {
        yield return new WaitForSeconds(1f);
        int index = UnityEngine.Random.Range(0, moveableTriangles.Count);
        var triangletoCapture = moveableTriangles[index];

        unitTriangles.Add(triangletoCapture);
        triangletoCapture.ChangeColor(pieceColor);
        Board.Instance.availableTriangles.Remove(triangletoCapture);
        callback(true);
    }




}