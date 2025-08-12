using UnityEngine;

public class PieceController : MonoBehaviour
{
    private Vector3 dragOffset;
    private float zCoord;
    private WorldBoard worldBoard;
    private bool isDragging;

    public void Initialize(WorldBoard board)
    {
        worldBoard = board;
    }

    public void StartDrag()
    {
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
            Debug.Log("Valid position! Placing piece.");
            worldBoard.PlacePiece(transform);
        }
        else
        {
            // Optional: Snap back to start position
            Debug.Log("Invalid position!");
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}