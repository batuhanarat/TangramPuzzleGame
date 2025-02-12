using UnityEngine;

[RequireComponent(typeof(Triangle))]
public class TriangleInteractionHandler : MonoBehaviour
{
    #region Private Variables

        private Piece ownerPiece;
        private Triangle triangle;
        private Vector3 dragOffset;
        private bool isDragging;

    #endregion

    private void Awake()
    {
        triangle = GetComponent<Triangle>();
    }

    private void OnMouseDown()
    {
        if (!CanStartDrag()) return;

        StartDrag();
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        UpdateDragPosition();
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;

        EndDrag();
    }

    private void EndDrag()
    {
        isDragging = false;
        ownerPiece.OnEndDrag();
    }

    private void UpdateDragPosition()
    {
        ownerPiece.SetPosition(GetMouseWorldPosition() + dragOffset);
    }

    private bool CanStartDrag()
    {
        ownerPiece = triangle.GetPiece();
        return ownerPiece != null && ownerPiece.CanBeDragged;
    }

    private void StartDrag()
    {
        isDragging = true;
        dragOffset = ownerPiece.transform.position - GetMouseWorldPosition();
        ownerPiece.OnStartDrag();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}