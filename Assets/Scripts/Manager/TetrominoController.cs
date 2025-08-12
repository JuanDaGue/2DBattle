using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TetrominoController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GridManager gridManager;
    private Vector3Int[] blockPositions;
    private Vector3 dragOffset;
    private int playerID;  // Track which player owns this piece
    private bool isDragging = false;
    private Vector3 originalPosition;

    void Start()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        originalPosition = transform.position;
        
        // Example L-shape positions (adjust based on your actual piece)
        blockPositions = new Vector3Int[] {
            new Vector3Int(0, 0, 0),
            new Vector3Int(1, 0, 0),
            new Vector3Int(2, 0, 0),
            new Vector3Int(2, 1, 0)
        };
    }

    public void SetPlayer(int player)
    {
        playerID = player;
        // Optional: Change color based on player
        GetComponentInChildren<SpriteRenderer>().color = 
            playerID == 1 ? Color.blue : Color.red;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Calculate offset between cursor and piece center
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        dragOffset = transform.position - worldPoint;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        
        // Convert screen position to world position with Z adjustment
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        cursorPosition.z = 0;  // Ensure Z=0 for 2D
        
        // Apply offset to maintain cursor-piece relationship
        transform.position = cursorPosition + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        SnapToGrid();
        
        if (IsPlacementValid())
        {
            PlaceTile();
        }
        else
        {
            // Return to original position if invalid
            transform.position = originalPosition;
        }
    }

    void SnapToGrid()
    {
        // Snap the entire piece to grid
        Vector3Int gridPos = gridManager.territoryTilemap.WorldToCell(transform.position);
        transform.position = gridManager.territoryTilemap.GetCellCenterWorld(gridPos);
    }

    bool IsPlacementValid()
    {
        // Check all blocks in the piece
        foreach (Vector3Int block in blockPositions)
        {
            Vector3 worldPos = transform.TransformPoint(block);
            Vector3Int gridPos = gridManager.territoryTilemap.WorldToCell(worldPos);
            
            // Boundary checks
            if (gridPos.x < 0 || gridPos.x >= gridManager.gridWidth || 
                gridPos.y < 0 || gridPos.y >= gridManager.gridHeight)
            {
                Debug.Log("Placement invalid: Out of bounds");
                return false;
            }
            
            // Optional: Add adjacency rules here
        }
        return true;
    }

    void PlaceTile()
    {
        // Set territory for all blocks in the piece
        foreach (Vector3Int block in blockPositions)
        {
            Vector3 worldPos = transform.TransformPoint(block);
            Vector3Int gridPos = gridManager.territoryTilemap.WorldToCell(worldPos);
            gridManager.SetTerritory(new Vector2Int(gridPos.x, gridPos.y), playerID);
        }

        // Notify turn manager
        //FindFirstObjectByType<TurnManager>().PiecePlaced();
        Destroy(gameObject);  // Remove piece after placement
    }
}