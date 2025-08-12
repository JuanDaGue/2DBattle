using UnityEngine;

public class WorldBoard : MonoBehaviour
{
    [Header("Board Settings")]
    public int boardWidth = 10;
    public int boardHeight = 20;
    
    [Header("References")]
    public GameObject boardTilePrefab;
    public Transform boardParent;
    
    [HideInInspector] public Transform[,] boardGrid;

    void Start()
    {
        InitializeGrid();
        CreateBoardTiles();
    }

    private void InitializeGrid()
    {
        boardGrid = new Transform[boardWidth, boardHeight];
    }

    private void CreateBoardTiles()
    {
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                Instantiate(boardTilePrefab, new Vector3(x, y, 0), Quaternion.identity, boardParent);
            }
        }
    }

    public bool IsValidPosition(Transform piece)
    {
        foreach (Transform block in piece)
        {
            Vector2Int pos = GetGridPosition(block.position);
            
            // Check boundaries
            if (pos.x < 0 || pos.x >= boardWidth || pos.y < 0 || pos.y >= boardHeight) return false;
            
            // Check if position is occupied
            if (pos.y < boardHeight && boardGrid[pos.x, pos.y] != null) return false;
        }
        return true;
    }

    public void PlacePiece(Transform piece)
    {
        foreach (Transform block in piece)
        {

            //Debug.Log("Placing block at: " + block.position);
            Vector2Int gridPos = GetGridPosition(block.position);

            if (gridPos.y < boardHeight)
            {
                boardGrid[gridPos.x, gridPos.y] = block;
                block.GetComponent<Block>().UpdateGridPosition();
                Instantiate(block.gameObject, new Vector3(gridPos.x, gridPos.y, 0), Quaternion.identity, boardParent);
                
            }
            else
            {
                Debug.LogWarning("Block above grid!");
            }
        }
        Destroy(piece.gameObject); // Remove piece controller
    }

    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x),
            Mathf.RoundToInt(worldPosition.y)
        );
    }
}