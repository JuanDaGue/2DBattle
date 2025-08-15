using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private TetrisSpawner spawner;
    private WorldBoard board;
    private Movement player1Movement;
    private Movement player2Movement;
    private DiceRoller diceRoller;

    // private TurnManager turnManager;
    // private GameState gameState;
    private bool isGameOver = false;
    private int turn = 1;
    private int player1Score = 0;
    private int player2Score = 0;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        // Start game
        isGameOver = false;
        turn = 1;
        player1Score = 0;
        player2Score = 0;
        StartCoroutine(SpawnTetromino());
    }

    public void SelectPlayer()
    {
    }
    public void StartTurn()
    {
        // Start turn
        turn = 1;
        SelectPlayer();

        StartCoroutine(SpawnTetromino());
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
        StartCoroutine(SpawnTetromino());
    }
    public void RollDice()
    {
        // Roll dice
        //StartCoroutine(diceRoller.RollDice());
    }

    public void EndTurn()
    {
        // End turn
        StartCoroutine(SpawnTetromino());
    }
    public void EndTurn(int playerID)
    {
        // End turn
        StartCoroutine(SpawnTetromino());
    }

    private IEnumerator SpawnTetromino()
    {
        //throw new NotImplementedException();
        yield return null;
    }

    public void EndGame()
    {
        // End game
        isGameOver = true;
        Debug.Log("Game over!");
    }


}
