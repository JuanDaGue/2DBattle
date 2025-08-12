using UnityEngine;

public class Block : MonoBehaviour
{
    public int x;
    public int y;
    private WorldBoard board;
    private PieceController pieceController;

    public void Initialize(WorldBoard board)
    {
        this.board = board;
        pieceController = GetComponentInParent<PieceController>();
    }

    private void OnMouseDown() => pieceController?.StartDrag();
    private void OnMouseDrag() => pieceController?.Drag();
    private void OnMouseUp() => pieceController?.EndDrag();
    
    // Updates grid position when moved
    public void UpdateGridPosition()
    {
        x = Mathf.RoundToInt(transform.position.x);
        y = Mathf.RoundToInt(transform.position.y);
    }
}