using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 1)]
    
public class Player : ScriptableObject
{
    [SerializeField] private string playerName;
    [SerializeField] private int playerID;
    [SerializeField] private int score;

    public string PlayerName => playerName;
    public int PlayerID => playerID;
    public int Score => score;
    public int TotalScore => score;
    [Header("Health")]

    public bool IsAlive => true;
    public bool IsDead => false;
    public bool canMove = false;

    // public bool IsTurn => GameManager.Instance.CurrentPlayerID == playerID;
    // public bool IsCurrentPlayer => GameManager.Instance.CurrentPlayerID == playerID;
    // public bool IsOpponentPlayer => GameManager.Instance.CurrentPlayerID != playerID;
    public int mana = 3;
    public int maxMana = 5;
    public void Initialize(string name, int id)
    {
        playerName = name;
        playerID = id;
        score = 0;
    }

    public void AddScore(int points)
    {
        score += points;
    }
}


