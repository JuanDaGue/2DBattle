using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour {
    public int gridWidth = 10, gridHeight = 10;
    public Tilemap territoryTilemap; // Visual tile layer
    public TileBase neutralTile, player1Tile, player2Tile;
    public Vector2Int player1BasePos = new Vector2Int(0, 0);
    public Vector2Int player2BasePos = new Vector2Int(9, 9);
    
    private int[,] gridState; // 0=neutral, 1=player1, 2=player2

    void Start() {
        InitializeGrid();
        PlaceBases();
    }

    void InitializeGrid() {
        gridState = new int[gridWidth, gridHeight];
        // Fill with neutral tiles
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                territoryTilemap.SetTile(new Vector3Int(x, y, 0), neutralTile);
            }
        }
    }

    void PlaceBases() {
        SetTerritory(player1BasePos, 1); // Player 1 base
        SetTerritory(player2BasePos, 2); // Player 2 base
    }

    public void SetTerritory(Vector2Int position, int playerId) {
        gridState[position.x, position.y] = playerId;
        territoryTilemap.SetTile(
            new Vector3Int(position.x, position.y, 0), 
            playerId == 1 ? player1Tile : player2Tile
        );
    }
}