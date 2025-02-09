using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private Color pieceColor;
    private List<Triangle> unitTriangles;
    private List<Triangle> moveableTriangles;

    public void AddToPiece(Triangle triangle)
    {
        unitTriangles.Add(triangle);
        triangle.GetAllNeighbors();
    }

    public void Init(Triangle firstTriangle, Color color)
    {
        this.pieceColor = color;
        unitTriangles = new()
        {
            firstTriangle
        };
        firstTriangle.ChangeColor(color);
        Board.Instance.availableTriangles.Remove(firstTriangle);
    }

    public void Capture()
    {
        Triangle triangle = Board.Instance.GetRandomTriangle();
        unitTriangles.Add(triangle);
        Board.Instance.availableTriangles.Remove(triangle);
    }

    public void Progress()
    {

    }



}