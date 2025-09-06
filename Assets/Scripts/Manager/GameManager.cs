using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject blockPrefab;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> playerSpawnPoints = new List<Transform>();
    [SerializeField] private List<Vector2Int> playerVectorSpawnPoints = new List<Vector2Int>(2);
    [SerializeField] private List<Transform> tetrominoSpawnPoints = new List<Transform>();

    [Header("Game Settings")]
    [SerializeField] [Range(1, 10)] private int player1Score = 0;
    [SerializeField] [Range(1, 10)] private int player2Score = 0;

    [Header("References")]
    [SerializeField] private WorldBoard board;
    [SerializeField] private TetrisSpawner pieceSpawners;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private TextMeshProUGUI gameStateText;

    [Header("Player Data")]
    [SerializeField] private List<Player> playerDataList = new List<Player>();
    [SerializeField] private List<Color> playerColors = new List<Color>();
    
    private Player currentPlayer;
    private DiceRoller diceRoller;
    private CamerasManager camerasManager;
    private int currentDiceRoll;
    private bool isGameOver = false;
    private int turn = 1;

    private void Awake()
    {
        // Find required components
        board = FindObjectOfType<WorldBoard>();
        diceRoller = FindObjectOfType<DiceRoller>();
        camerasManager = FindObjectOfType<CamerasManager>();
        
        if (board == null) Debug.LogError("WorldBoard not found!");
        if (diceRoller == null) Debug.LogError("DiceRoller not found!");
        if (camerasManager == null) Debug.LogError("CamerasManager not found!");
        if (pieceSpawners == null) Debug.LogError("TetrisSpawner not found!");

        // Initialize spawners
        pieceSpawners.Initialize(board, blockPrefab);
        
        // Set player spawn points in grid coordinates
        int player1X = Mathf.RoundToInt(board.boardWidth * 0.5f);
        int player1Y = 0;
        int player2X = Mathf.RoundToInt(board.boardWidth * 0.5f);
        int player2Y = board.boardHeight - 1;

        playerVectorSpawnPoints.Add(new Vector2Int(player1X, player1Y));
        playerVectorSpawnPoints.Add(new Vector2Int(player2X, player2Y));
        
        // Initialize UI for first player
        if (playerDataList.Count > 0 && uiManager != null)
        {
            uiManager.Initialize(playerDataList[0]);
        }
    }

    private void Start()
    {
        SpawnPlayers();
        StartGame();
    }

    private void SpawnPlayers()
    {
        if (camerasManager != null)
        {
            camerasManager.cameraTargets.Clear(); // Clear any existing targets
        }

        for (int i = 0; i < playerDataList.Count && i < playerSpawnPoints.Count; i++)
        {
            Player playerData = playerDataList[i];
            Transform spawnPoint = playerSpawnPoints[i];

            GameObject playerGO = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            playerGO.name = $"Player_{playerData.PlayerName}_{playerData.PlayerID}";
            playerGO.transform.localScale = Vector3.one * 0.5f;

            // Set player color if available
            if (i < playerColors.Count)
            {
                Renderer renderer = playerGO.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = playerColors[i];
                }
            }

            // Initialize movement component
            Movement movement = playerGO.GetComponent<Movement>();
            if (movement != null)
            {
                movement.Initialize(playerData, board, diceRoller);
            }

            // Set player position and add to camera targets
            playerGO.transform.position = new Vector3(
                playerVectorSpawnPoints[i].x, 
                playerVectorSpawnPoints[i].y, 
                0
            );
            
            if (camerasManager != null)
            {
                camerasManager.cameraTargets.Add(playerGO.transform);
            }
        }
    }

    public void StartGame()
    {
        turn = 1;
        isGameOver = false;
        player1Score = 0;
        player2Score = 0;
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (!isGameOver)
        {
            yield return StartCoroutine(ShowGameStateText("Game Started"));
            yield return StartCoroutine(StartTurn());
            yield return StartCoroutine(ShowGameStateText("Waiting for block set"));
            yield return StartCoroutine(WaitForBlockSet());
            yield return StartCoroutine(ShowGameStateText("Rolling dice"));
            yield return StartCoroutine(RollDice());
            
            if (currentPlayer != null && currentDiceRoll > 0)
            {
                currentPlayer.canMove = true;
                currentPlayer.SetMovePoints(currentDiceRoll);
                yield return new WaitUntil(() => !currentPlayer.canMove);
            }
            
            yield return StartCoroutine(ShowGameStateText("Ending turn"));
            yield return StartCoroutine(EndTurn());
            yield return StartCoroutine(ShowGameStateText("Next Turn"));
            
            // Check for game over condition
            if (turn > 20) // Example condition
            {
                EndGame();
            }
        }
    }

    private IEnumerator StartTurn()
    {
        Debug.Log($"--- Turn {turn} ---");
        SelectPlayer(turn);
        SpawnTetromino();
        yield return null;
    }

    private void SelectPlayer(int turn)
    {
        int playerIndex = (turn - 1) % playerDataList.Count;
        if (playerIndex < playerDataList.Count)
        {
            currentPlayer = playerDataList[playerIndex];
            Debug.Log($"It's {currentPlayer.PlayerName}'s turn (ID: {currentPlayer.PlayerID})");
            
            if (camerasManager != null)
            {
                camerasManager.SwitchCameraToPlayer(currentPlayer.PlayerID);
            }
            
            if (uiManager != null)
            {
                uiManager.Initialize(currentPlayer);
            }
        }
    }

    private IEnumerator RollDice()
    {
        if (diceRoller != null)
        {
            diceRoller.RollDice();
            yield return new WaitUntil(() => diceRoller.HasResult);
            currentDiceRoll = diceRoller.GetResult();
        }
        yield return null;
    }

    private IEnumerator EndTurn()
    {
        if (currentPlayer != null)
        {
            currentPlayer.AddMana(1);
            currentPlayer.AddScore(Random.Range(1, 5));
        }
        turn++;
        yield return null;
    }

    private void SpawnTetromino()
    {
        if (pieceSpawners != null && currentPlayer != null)
        {
            int spawnerIndex = currentPlayer.PlayerID - 1;
            if (spawnerIndex < tetrominoSpawnPoints.Count)
            {
                pieceSpawners.SetSpawnPoint(tetrominoSpawnPoints[spawnerIndex]);
                pieceSpawners.SpawnPieceByType();
            }
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        Debug.Log("Game over!");
        StartCoroutine(ShowGameStateText("Game Over!"));
    }

    private IEnumerator WaitForBlockSet()
    {
        if (board != null)
        {
            while (!board.blockSeted)
            {
                yield return null;
            }
            board.blockSeted = false;
        }
    }

    private IEnumerator ShowGameStateText(string gameState)
    {
        if (gameStateText != null)
        {
            gameStateText.text = gameState;
            gameStateText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            gameStateText.gameObject.SetActive(false);
        }
    }
}