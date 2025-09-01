using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/Player", order = 1)]
public class Player : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string playerName;
    [SerializeField] private int playerID;
    [SerializeField] private int score;

    [Header("Lives")]
    [SerializeField] private int maxLives = 5;
    [SerializeField] private int lives=3;

    [Header("Mana")]
    [SerializeField] private int maxMana = 5;
    [SerializeField] private int mana;

    [Header("Movement")]
    public bool    canMove;
    public int     movePoints;
    public int     x, y;

    [Header("Combat Settings")]
    [SerializeField] private int rangedAttackRange = 3;

    // Read-only properties
    public string PlayerName => playerName;
    public int    PlayerID   => playerID;
    public int    Score      => score;
    public int    TotalScore => score;
    public int    Lives      => lives;
    public int    MaxLives   => maxLives;
    public int    Mana       => mana;
    public bool   IsAlive    => lives > 0;
    public bool   IsDead     => lives == 0;

    // Called once when you create or reset this player
    public void Initialize(string name, int id)
    {
        playerName = name;
        playerID   = id;
        score      = 0;

        lives      = maxLives;
        mana       = 0;

        canMove    = false;
        movePoints = 0;

        x = 0;
        y = 0;
    }

    // Score
    public void AddScore(int points)
    {
        score += points;
    }

    // Lives management
    public void TakeDamage(int lifeLoss = 1)
    {
        if (!IsAlive) return;

        lives -= lifeLoss;
        if (lives <= 0)
        {
            lives = 0;
            // GameManager.Instance.EndGame();
        }
    }

    public void AddLife(int amount = 1)
    {
        if (!IsAlive) return;

        lives += amount;
        if (lives > maxLives)
            lives = maxLives;
    }

    // Mana management
    public void AddMana(int amount)
    {
        mana += amount;
        if (mana > maxMana)
            mana = maxMana;
    }

    public void TakeMana(int amount)
    {
        mana -= amount;
        if (mana < 0)
            mana = 0;
    }

    public void RestoreMana(int amount)
    {
        AddMana(amount);
    }

    // Movement points
    public void SetMovePoints(int points)
    {
        movePoints = points;
        canMove    = movePoints > 0;
    }

    public void ResetMovePoints()
    {
        movePoints = 0;
        canMove    = false;
    }

    public void SetPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // Basic melee attack: costs 1 life on the target
    public void BasicAttack(Player target)
    {
        if (!IsAlive || target == null || target.IsDead) return;
        target.TakeDamage(1);
    }

    // Ranged attack: costs 1 life on the target if in range
    public bool RangedAttack(Player target)
    {
        if (!IsAlive || target == null || target.IsDead) 
            return false;

        float distance = Vector2.Distance(
            new Vector2(x, y),
            new Vector2(target.x, target.y)
        );

        if (distance <= rangedAttackRange)
        {
            target.TakeDamage(1);
            return true;
        }

        return false;
    }
}