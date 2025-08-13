using System.Collections;
using System.IO;
using UnityEngine;

public class WorldBoard : MonoBehaviour
{
    [Header("Board Settings")]
    public int boardWidth = 10;
    public int boardHeight = 20;

    [Header("References")]
    public GameObject boardTilePrefab;
    public Transform boardParent;
    // public Movement player1Movement;
    // public Movement player2Movement;

    Tile[,] boardGrid;
    Block[,] pathGrid;

    void Start()
    {
        InitializeGrid();
        CreateBoardTiles();

    }

    private void InitializeGrid()
    {
        boardGrid = new Tile[boardWidth, boardHeight];
        pathGrid = new Block[boardWidth, boardHeight];
    }

    private void CreateBoardTiles()
    {
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                var tile = Instantiate(boardTilePrefab, new Vector3(x, y, 0), Quaternion.identity, boardParent);

                // tile.GetComponent<Tile>().Setup(x, y, this);
                // boardGrid[x, y] = tile.transform;
                boardGrid[x, y] = tile.GetComponent<Tile>();
                boardGrid[x, y]?.Setup(x, y, this);

            }
        }
    }

    public bool IsValidPosition(Transform piece)
    {
        foreach (Transform block in piece)
        {
            Vector2Int pos = GetGridPosition(block.position);
            if (!IsPositionValid(pos)) return false;
        }
        return true;
    }
    public bool IsWalkable(Vector2Int position)
    {
        if (position.x < 0 || position.x >= boardWidth || position.y < 0 || position.y >= boardHeight) return false;
        Debug.Log("Is walkable? " + pathGrid[position.x, position.y]);
        return pathGrid[position.x, position.y] != null;
    }
    public void PlacePiece(PieceController pieceController)
    {
        Transform piece = pieceController.transform;
        foreach (Transform blockTransform in piece.transform)
        {
            Block block1 = blockTransform.GetComponent<Block>();
            Vector2Int gridPos = GetGridPosition(blockTransform.position);
            Debug.Log("Placing block at " + gridPos);

            if (IsPositionValid(gridPos))
            {
                var blockObj = Instantiate(block1.gameObject, new Vector3(gridPos.x, gridPos.y, 0), Quaternion.identity, boardParent);
                Block block = blockObj.GetComponent<Block>();

                if (block == null) continue;
                // Update block's internal grid position
                block.Setup(gridPos.x, gridPos.y, this);
                // Place in path grid
                pathGrid[gridPos.x, gridPos.y] = block;
                // Update world position to grid-aligned position
                blockTransform.position = new Vector3(gridPos.x, gridPos.y, 0);
            }
            else
            {
                Debug.LogWarning($"Invalid position at {gridPos}");
            }
        }

        // Destroy the piece controller but keep blocks
        Destroy(pieceController.gameObject);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x),
            Mathf.RoundToInt(worldPosition.y)
        );
    }
    private bool IsPositionValid(Vector2Int gridPos)
    {
        return gridPos.x >= 0 &&
            gridPos.x < boardWidth &&
            gridPos.y >= 0 &&
            gridPos.y < boardHeight &&
            pathGrid[gridPos.x, gridPos.y] == null;
    }
    IEnumerator CheckMatch(Vector2Int position)
    {
        // Check for matches in the path grid
        // This is a placeholder for match checking logic
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                if (pathGrid[x, y] != null)
                {
                    // Check if this block matches the criteria
                    // For example, check color or type
                    // If a match is found, handle it accordingly

                    
                }
            }
        }

        yield return null;
    }
}   