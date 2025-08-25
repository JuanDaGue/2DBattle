using System.Collections;
using System.Collections.Generic;
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
    public bool blockSeted = false;
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
        //Debug.Log("Is walkable? " + pathGrid[position.x, position.y]);
        return pathGrid[position.x, position.y] != null;
    }
    public void PlacePiece(PieceController pieceController)
    {
        Transform piece = pieceController.transform;
        foreach (Transform blockTransform in piece.transform)
        {
            Block block1 = blockTransform.GetComponent<Block>();
            Vector2Int gridPos = GetGridPosition(blockTransform.position);
            //Debug.Log("Placing block at " + gridPos);

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
        StartCoroutine(CheckMatch());
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
    private IEnumerator CheckMatch()
    {
        // Check for matches in the path grid
        // This is a placeholder for match checking logic
        yield return null;
        List<Block> blockToRemove = new List<Block>();
        List<Vector2Int> alldirectors = new List<Vector2Int>() { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                if (pathGrid[x, y] != null)
                {
                    //GetMatch(x, y);
                    //Debug.Log($"Checking match at {x}, {y}");
                    foreach (Vector2Int direction in alldirectors)
                    {
                        List<Block> match = GetMatchByDirection(x, y, direction);
                        if (match != null)
                        {
                            blockToRemove.AddRange(match);
                        }
                    }
                }
            }
        }
        foreach (Block block in blockToRemove)
        { 
            RemoveBlock(block);
        }
        yield return null;
    }

    public List<Block> GetMatchByDirection(int posx, int posy, Vector2Int direction, int matchLength = 4)
    {
        Vector2Int position = new Vector2Int(posx, posy);
        List<Block> matchBlocks = new List<Block>();
        if (pathGrid[position.x, position.y] == null) return null;

        // Get all blocks in the match direction
        int x = position.x;
        int y = position.y;

        while (x >= 0 && x < boardWidth && y >= 0 && y < boardHeight && pathGrid[x, y] != null)
        {
            matchBlocks.Add(pathGrid[x, y]);
            x += direction.x;
            y += direction.y;
        }
        //Debug.Log($"Match length: {matchBlocks.Count} at {posx}, {posy} in direction {direction}");
        if (matchBlocks.Count > matchLength)
        {
            return matchBlocks;
        }
        return null;
    }

    private void RemoveBlock(Block block)
    {
        pathGrid[block.x, block.y] = null;
        Destroy(block.gameObject);
    }

}   