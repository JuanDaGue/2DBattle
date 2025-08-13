using UnityEngine;

public class Block : MonoBehaviour
{
    //[HideInInspector] 
    public int x;
    //[HideInInspector] 
    public int y;
    [HideInInspector] public WorldBoard board;
    private PieceController pieceController;
    public void Initialize(WorldBoard board)
    {
        this.board = board;
        pieceController = GetComponentInParent<PieceController>();
    }

    public void Setup(int x, int y, WorldBoard board)
    {
        this.x = x;
        this.y = y;
        this.board = board;
        
        // Only get controller if we're part of a piece
        pieceController = GetComponentInParent<PieceController>();
    }

    // Only allow dragging if part of an active piece
    private void OnMouseDown() {
        if (pieceController != null) pieceController.StartDrag();
    }
    
    private void OnMouseDrag() {
        if (pieceController != null) pieceController.Drag();
    }
    
    private void OnMouseUp() {
        if (pieceController != null) pieceController.EndDrag();
    }
}