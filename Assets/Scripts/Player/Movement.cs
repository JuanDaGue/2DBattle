using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private WorldBoard worldBoard;

    private Player playerData;
    private DiceRoller diceRoller;
    private bool isMoving = false;
    private Vector2Int currentDirection = Vector2Int.zero;

    private void Update()
    {
        if (isMoving || playerData == null || !playerData.canMove) return;
        
        HandleInput();
    }

    public void Initialize(Player data, WorldBoard board, DiceRoller dice)
    {
        playerData = data;
        worldBoard = board;
        diceRoller = dice;
        
        Debug.Log($"Initialized movement for {playerData.PlayerName}");
    }

    private void HandleInput()
    {
        if (playerData.movePoints <= 0) 
        {
            playerData.canMove = false;
            return;
        }

        Vector2Int newDirection = GetInputDirection();
        
        if (newDirection != Vector2Int.zero && newDirection != currentDirection)
        {
            currentDirection = newDirection;
            Vector2Int currentGrid = worldBoard.GetGridPosition(transform.position);
            Vector2Int targetGrid = currentGrid + currentDirection;
            
            if (worldBoard.IsWalkable(targetGrid))
            {
                StartCoroutine(MoveToCell(targetGrid));
            }
            else
            {
                Debug.Log($"Cannot move to {targetGrid} - position is not walkable");
            }
        }
    }

    private Vector2Int GetInputDirection()
    {
        if (Input.GetKeyDown(KeyCode.W)) return Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) return Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) return Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) return Vector2Int.right;
        
        return Vector2Int.zero;
    }

    private IEnumerator MoveToCell(Vector2Int targetGrid)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(targetGrid.x, targetGrid.y, startPos.z);
        float distance = Vector3.Distance(startPos, endPos);
        float duration = distance / speed;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
        
        // Update move points and UI
        playerData.movePoints--;
        if (diceRoller != null)
        {
            diceRoller.UpdateUI(playerData.movePoints);
        }
        
        Debug.Log($"Moved to {targetGrid}, remaining move points: {playerData.movePoints}");
        
        // Check if movement should continue
        if (playerData.movePoints <= 0)
        {
            playerData.canMove = false;
        }
    }
    
    public void AddScore(int points)
    {
        if (playerData != null)
        {
            playerData.AddScore(points);
        }
    }
}