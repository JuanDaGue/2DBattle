using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject blockPrefab;

    [Header("Spawn Points")]
    public List<Transform> playerSpawnPoints = new List<Transform>();
    public List<Vector2Int> playerVectorSpawnPoints = new List<Vector2Int>(2);
    public List<Transform> tetrominoSpawnPoints = new List<Transform>();

    [Header("Game Settings")]
    [Range(1, 10)]
    public int player1Score = 0;
    [Range(1, 10)]
    public int player2Score = 0;

    [Header("References")]
    [SerializeField] private WorldBoard board;
    [SerializeField] private TetrisSpawner pieceSpawners;

    [Header("Player Data")]
    [SerializeField] private List<Player> playerDataList = new List<Player>();
    private Player currentPlayer;
    private DiceRoller diceRoller;
    private int currentDiceRoll;

    private bool isGameOver = false;
    private int turn = 1;
    public List<Color> playerColors = new List<Color>();
    public TextMeshProUGUI gameStateText;
    public UiManager uiManager;

    [Header("References")]
    private CamerasManager camerasManager;

    private void Awake()
    {
        board = FindFirstObjectByType<WorldBoard>();
        diceRoller = FindFirstObjectByType<DiceRoller>();
        camerasManager = FindFirstObjectByType<CamerasManager>();

        // Initialize spawners for each player

        pieceSpawners.Initialize(board, blockPrefab);
        // Set player spawn points in grid coordinates
        int player1X = (int)(board.boardWidth * 0.5);
        int player1Y = 0;
        int player2X = (int)(board.boardWidth * 0.5);
        int player2Y = (int)(board.boardHeight) - 1;

        playerVectorSpawnPoints.Add(new Vector2Int(player1X, player1Y));
        playerVectorSpawnPoints.Add(new Vector2Int(player2X, player2Y));
        uiManager.Initialize(playerDataList[0]);
       }

    

    void Start()
    {

        SpawnPlayers();
        StartGame();
    }

private void SpawnPlayers()
{
    camerasManager.cameraTargets.Clear(); // Clear any existing targets

        for (int i = 0; i < playerDataList.Count && i < playerSpawnPoints.Count; i++)
        {
            Player playerData = playerDataList[i];
            Transform spawnPoint = playerSpawnPoints[i];

            GameObject playerGO = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
            playerGO.transform.localScale = Vector3.one * 0.5f;
            playerGO.name = $"Player_{playerData.PlayerID}";

            if (i < playerColors.Count)
                playerGO.GetComponent<Renderer>().material.color = playerColors[i];

            Movement movement = playerGO.GetComponent<Movement>();
            if (movement != null)
            {
                movement.Initialize(playerData, board, diceRoller);
            }

            // Set player position and add to camera targets
            playerGO.transform.position = new Vector3(playerVectorSpawnPoints[i].x, playerVectorSpawnPoints[i].y, 0);
            camerasManager.cameraTargets.Add(playerGO.transform);
            
                
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
        yield return StartCoroutine(UIGameText("Game Started"));
        yield return StartCoroutine(StartTurn());
        yield return StartCoroutine(UIGameText("Waiting for block set"));
        yield return StartCoroutine(WaitForBlockSet());
        yield return StartCoroutine(UIGameText("Rolling dice"));
        yield return StartCoroutine(RollDice());
        

        if (currentPlayer != null && currentDiceRoll > 0)
        {
            currentPlayer.canMove = true;
            currentPlayer.SetMovePoints(currentDiceRoll);
            yield return new WaitUntil(() => !currentPlayer.canMove);
        }
        
        yield return StartCoroutine(UIGameText("Ending turn"));
        yield return StartCoroutine(EndTurn());
        yield return StartCoroutine(UIGameText("Next Turn"));
        if (!isGameOver)
            StartCoroutine(GameLoop());
    }

    private IEnumerator StartTurn()
    {
        Debug.Log($"--- Turn {turn} ---");
        SelectPlayer(turn);
        SpawnTetromino();
        yield return null;
    }

    public void SelectPlayer(int turn)
    {
        int playerIndex = (turn - 1) % playerDataList.Count;
        currentPlayer = playerDataList[playerIndex];
        Debug.Log($"It's {currentPlayer.PlayerName}'s turn (ID: {currentPlayer.PlayerID})");
        camerasManager.SwitchCameraToPlayer(currentPlayer.PlayerID);
        uiManager.Initialize(currentPlayer);
    }

    private IEnumerator RollDice()
    {
        diceRoller.RollDice();
        yield return new WaitUntil(() => diceRoller.HasResult);
        currentDiceRoll = diceRoller.GetResult();
        yield return null;
    }

    private IEnumerator EndTurn()
    {
        currentPlayer.AddMana(1);
        currentPlayer.AddScore(Random.Range(1, 5));
        turn++;
        yield return null;
    }

    private void SpawnTetromino()
    {
        int spawnerIndex = currentPlayer.PlayerID - 1;
        pieceSpawners.SetSpawnPoint(tetrominoSpawnPoints[spawnerIndex]);
        pieceSpawners.SpawnPieceByType();
    }
    public void EndGame()
    {
        isGameOver = true;
        Debug.Log("Game over!");
    }

    private IEnumerator WaitForBlockSet()
    {
        while (!board.blockSeted)
        {
            yield return null;
        }
        board.blockSeted = false;
    }
    private IEnumerator UIGameText( string gameState)
    {
        gameStateText.text = gameState;
        gameStateText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameStateText.gameObject.SetActive(false);
    }
}