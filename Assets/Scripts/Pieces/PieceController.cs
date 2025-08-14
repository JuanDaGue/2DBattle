using UnityEngine;

public class PieceController : MonoBehaviour
{
    private Vector3 dragOffset;
    private float zCoord;
    private WorldBoard worldBoard;
    private bool isDragging;
    private Vector3 originalPosition;

    public void Initialize(WorldBoard board)
    {
        worldBoard = board;
    }

    public void StartDrag()
    {
        originalPosition = transform.position;
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        dragOffset = transform.position - GetMouseWorldPos();
        isDragging = true;
    }

    public void Drag()
    {
        if (!isDragging) return;
        
        Vector3 newPosition = GetMouseWorldPos() + dragOffset;
        transform.position = new Vector3(newPosition.x, newPosition.y, 0);
    }

    public void EndDrag()
    {
        if (!isDragging) return;
        isDragging = false;
        
        if (worldBoard.IsValidPosition(transform))
        {
            worldBoard.PlacePiece(this);
        }
        else
        {
            // Snap back to original position
            transform.position = originalPosition;
            //Debug.Log("Invalid position! Snapping back.");
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}