using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject blockPrefab;

    [Header("Spawn Points")]
    public List<Transform> player1SpawnPoint = new List<Transform>(2);
    public List<Transform> TetrominoSpawnPoint = new List<Transform>(2);
    [Header("Game Settings")]
    [Range(1, 10)]
    public int player1Score = 0;
    [Range(1, 10)]
    public int player2Score = 0;
    [Header("References")]
    [SerializeField] private WorldBoard board;
    [SerializeField] private TetrisSpawner spawner;

    [Header("Player Data")]
    [SerializeField] private List<Player> playerDataList = new List<Player>();

    private Movement player1Movement;
    private Movement player2Movement;
    private DiceRoller diceRoller;


    // private TurnManager turnManager;
    // private GameState gameState;
    private bool isGameOver = false;
    private int turn = 1;




    public List<Color> player1Colors = new List<Color>(2);

    private void Awake()
    {
        // // Initialize the board and spawner
        // board = FindFirstObjectByType<WorldBoard>();
        // if (board == null)
        // {
        //     Debug.LogError("WorldBoard not found in the scene.");
        //     return;
        // }

        // spawner = FindFirstObjectByType<TetrisSpawner>();
        // if (spawner == null)
        // {
        //     Debug.LogError("TetrisSpawner not found in the scene.");
        //     return;
        // }

        // Initialize the spawner with the board and spawn points
        spawner.Initialize(board, blockPrefab, TetrominoSpawnPoint[0]);
    }
    void Start()
    {
        spwanPlayers();
        StartGame();
    }

 private void spwanPlayers()
{
    for (int i = 0; i < playerDataList.Count && i < player1SpawnPoint.Count; i++)
    {
        Player playerData = playerDataList[i];
        Transform spawnPoint = player1SpawnPoint[i];

        GameObject playerGO = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        playerGO.transform.localScale = Vector3.one;
        playerGO.transform.rotation = Quaternion.identity;
        playerGO.transform.SetParent(spawnPoint);
        playerGO.name = $"Player_{playerData.PlayerID}";

        // Assign color if available
        if (i < player1Colors.Count)
            playerGO.GetComponent<Renderer>().material.color = player1Colors[i];

        // Link data to behavior
        Movement movement = playerGO.GetComponent<Movement>();
        if (movement != null)
        {
            movement.Initialize(playerData,board); // You’ll need to implement this method
        }

        Debug.Log($"Spawned {playerData.PlayerName} with ID {playerData.PlayerID}");
    }
}


    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        // Start game
        isGameOver = false;
        Debug.Log("Game started!");
        Debug.Log("Player 1 score: " + turn);
        player1Score = 0;
        player2Score = 0;
        StartTurn();
    }

    public void SelectPlayer()
    {

    }
    public void StartTurn()
    {
        // Start turn
        turn = 1;
        //SelectPlayer();

        SpawnTetromino();
        // SetTetromino();
        // RollDice();
        // PowerUP();
        // MovemethePlayer();
        // AddTRmpas()
        

    }
    public void NextTurn()
    {
        // Next turn
        turn++;

    }
    public void RollDice()
    {
        // Roll dice
        //StartCoroutine(diceRoller.RollDice());
    }

    public void EndTurn()
    {
        // End turn
        //StartCoroutine(SpawnTetromino());
    }
public void EndTurn(int playerID)
{
    Player currentPlayer = playerDataList.Find(p => p.PlayerID == playerID);
    if (currentPlayer != null)
    {
        currentPlayer.AddScore(UnityEngine.Random.Range(1, 5));
        Debug.Log($"Player {currentPlayer.PlayerName} now has score {currentPlayer.Score}");
    }
}

    private void SpawnTetromino()
    {
        spawner.SpawnPieceByType();
    }

    public void EndGame()
    {
        // End game
        isGameOver = true;
        Debug.Log("Game over!");
    }


}
