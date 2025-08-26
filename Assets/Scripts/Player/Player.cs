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
    public int health = 100;
    public int maxHealth = 100;
    public bool canMove = false;
    public int movePoints = 0;
    public int x,y;
    [Header("Mana")]
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
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            // GameManager.Instance.EndGame();
        }
    }
    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }
    public void TakeMana(int amount)
    {
        mana -= amount;
        if (mana <= 0)
        {
            mana = 0;
            // GameManager.Instance.EndGame();
        }
    }
    public void RestoreMana(int amount)
    {
        mana += amount;
        if (mana > maxMana)
            mana = maxMana;
    }
    public void SetMovePoints(int points)
    {
        movePoints = points;
        canMove = movePoints > 0;
        //Debug.Log($"Player {PlayerName} can move: {canMove}");
    }
    public void ResetMovePoints()
    {
        movePoints = 0;
        canMove = false;
    }
    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;

    }
}


