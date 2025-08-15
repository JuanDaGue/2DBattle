using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10, gridHeight = 10;
    public Tilemap territoryTilemap; // Visual tile layer
    public TileBase neutralTile, player1Tile, player2Tile;
    public Vector2Int player1BasePos = new Vector2Int(0, 0);
    public Vector2Int player2BasePos = new Vector2Int(9, 9);

    private int[,] gridState; // 0=neutral, 1=player1, 2=player2

    void Start()
    {
        InitializeGrid();
        PlaceBases();
    }

    void InitializeGrid()
    {
        gridState = new int[gridWidth, gridHeight];
        // Fill with neutral tiles
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                territoryTilemap.SetTile(new Vector3Int(x, y, 0), neutralTile);
            }
        }
    }

    void PlaceBases()
    {
        SetTerritory(player1BasePos, 1); // Player 1 base
        SetTerritory(player2BasePos, 2); // Player 2 base
    }

    public void SetTerritory(Vector2Int position, int playerId)
    {
        gridState[position.x, position.y] = playerId;
        territoryTilemap.SetTile(
            new Vector3Int(position.x, position.y, 0),
            playerId == 1 ? player1Tile : player2Tile
        );
    }
}



// using System.Collections;
// using UnityEngine;

// public enum Player { Player1, Player2 }

// public class GameManager : MonoBehaviour
// {
//     [Header("Player References")]
//     public Movement player1Movement;
//     public Movement player2Movement;
//     public Transform player1SpawnPoint;
//     public Transform player2SpawnPoint;

//     [Header("Game Components")]
//     public TetrisSpawner tetrisSpawner;
//     public DiceRoller diceRoller;
//     public WorldBoard worldBoard;
//     public PowerUpManager powerUpManager;
//     public TrapManager trapManager;

//     [Header("Game Settings")]
//     public int winScore = 100;
//     public float turnDelay = 1f;

//     private Player currentPlayer;
//     private int currentDiceRoll;
//     private bool isPlayerTurnActive = true;
//     private bool isGameActive = true;

//     void Start()
//     {
//         InitializeGame();
//     }

//     private void InitializeGame()
//     {
//         currentPlayer = Player.Player1;
//         player1Movement.transform.position = player1SpawnPoint.position;
//         player2Movement.transform.position = player2SpawnPoint.position;
//         player1Movement.Initialize(worldBoard);
//         player2Movement.Initialize(worldBoard);
        
//         StartCoroutine(GameLoop());
//     }

//     private IEnumerator GameLoop()
//     {
//         while (isGameActive)
//         {
//             yield return StartCoroutine(PlayerTurnRoutine(currentPlayer));
//             SwitchPlayer();
//         }
//     }

//     private IEnumerator PlayerTurnRoutine(Player player)
//     {
//         Debug.Log($"{player}'s turn starting");
        
//         // 1. Spawn tetromino
//         SpawnTetrominoForPlayer(player);
//         yield return new WaitUntil(() => worldBoard.IsPiecePlaced);
        
//         // 2. Roll dice
//         yield return StartCoroutine(RollDice());
//         Debug.Log($"{player} rolled: {currentDiceRoll}");
        
//         // 3. Move player
//         Movement currentMovement = GetPlayerMovement(player);
//         yield return StartCoroutine(currentMovement.MovePlayer(currentDiceRoll));
        
//         // 4. Apply tile effects
//         yield return StartCoroutine(ApplyTileEffects(player));
        
//         // 5. Apply traps/power-ups
//         yield return StartCoroutine(ApplySpecialEffects(player));
        
//         Debug.Log($"{player}'s turn ended");
//         yield return new WaitForSeconds(turnDelay);
//     }

//     private void SpawnTetrominoForPlayer(Player player)
//     {
//         tetrisSpawner.transform.position = (player == Player.Player1) ? 
//             player1SpawnPoint.position : player2SpawnPoint.position;
        
//         tetrisSpawner.SpawnPieceByType();
//     }

//     private IEnumerator RollDice()
//     {
//         diceRoller.RollDice();
//         yield return new WaitUntil(() => diceRoller.HasResult);
//         currentDiceRoll = diceRoller.GetResult();
//         diceRoller.ResetDice();
//     }

//     private Movement GetPlayerMovement(Player player)
//     {
//         return (player == Player.Player1) ? player1Movement : player2Movement;
//     }

//     private IEnumerator ApplyTileEffects(Player player)
//     {
//         // Get the tile the player landed on
//         Vector2Int playerPos = GetPlayerMovement(player).CurrentGridPosition;
//         Tile landedTile = worldBoard.GetTileAt(playerPos);
        
//         if (landedTile != null)
//         {
//             yield return StartCoroutine(landedTile.ApplyEffect(player));
//         }
//     }

//     private IEnumerator ApplySpecialEffects(Player player)
//     {
//         // Chance to spawn power-up or trap
//         if (Random.value > 0.7f) // 30% chance
//         {
//             if (Random.value > 0.5f)
//             {
//                 powerUpManager.SpawnPowerUp(GetPlayerMovement(player).CurrentGridPosition);
//             }
//             else
//             {
//                 trapManager.PlaceTrap(GetPlayerMovement(player).CurrentGridPosition);
//             }
//         }
        
//         yield return null;
//     }

//     private void SwitchPlayer()
//     {
//         currentPlayer = (currentPlayer == Player.Player1) ? 
//             Player.Player2 : Player.Player1;
//     }

//     public void AddScore(Player player, int points)
//     {
//         if (player == Player.Player1)
//         {
//             player1Score += points;
//             if (player1Score >= winScore) EndGame(Player.Player1);
//         }
//         else
//         {
//             player2Score += points;
//             if (player2Score >= winScore) EndGame(Player.Player2);
//         }
//     }

//     private void EndGame(Player winner)
//     {
//         isGameActive = false;
//         Debug.Log($"Game Over! {winner} wins!");
//         // Implement game over UI or scene transition
//     }
// }