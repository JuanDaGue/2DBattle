using UnityEngine;
[CreateAssetMenu(fileName = "PowerUp", menuName = "ScriptableObjects/PowerUp", order = 1)]

public class PowerUp : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string powerUpName;
    [SerializeField] private int powerUpID;
    [SerializeField] private int cost;


    [Header("Movement")]
    public bool    IsActivate;
    //public int     movePoints;
    public int     x, y;
    public GameObject PowerUpPrefabs;
    public Sprite Icon;
    [Header("Combat Settings")]
    [SerializeField] private int rangedAttackRange = 3;

    // Read-only properties
    public string PowerUpName => PowerUpName;
    public int    PowerUpID   => PowerUpID;
    public int    Cost      => cost;
  

    public  virtual void Activar(){
        Debug.Log("Activate the poewer Uo");
    }
    public void Setup(){

    }

 
}

enum powerUpType
{
    Health,
    Mana,
    Speed,
    Damage,
    Defense,
    RangedAttack,
    Attack,
    Movement,
    Life,
    Trap,
}