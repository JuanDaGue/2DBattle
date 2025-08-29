using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private WorldBoard worldBoard;

    [SerializeField] private bool isMoving = false;
    private Player playerData;
    private DiceRoller diceRoller;
    private  int movePoints = 0;
    void Update()
    {
        if (isMoving) return;
        if (playerData == null) return;

        if (!playerData.canMove) return;    
        
        HandleInput();
    }
    public void Initialize(Player data, WorldBoard board, DiceRoller dice)
    {
        playerData = data;
        worldBoard = board;
        diceRoller = dice;
        movePoints = playerData.movePoints;
        // Optionally use playerData to set initial state
        // Debug.Log($"Initialized movement for {playerData.PlayerName}");
        // Debug.Log($"Player can move: {playerData.canMove}");
        
    }


    private void HandleInput()
    {
        //Debug.Log("Handling input for movement"+ isMoving);
        
        //Debug.Log("Move Points: " +playerData.movePoints);
        if (playerData.movePoints > 0)
        {
            ;
            Vector2Int dir = Vector2Int.zero;
            if (Input.GetKeyDown(KeyCode.W)) dir = Vector2Int.up;
            else if (Input.GetKeyDown(KeyCode.S)) dir = Vector2Int.down;
            else if (Input.GetKeyDown(KeyCode.A)) dir = Vector2Int.left;
            else if (Input.GetKeyDown(KeyCode.D)) dir = Vector2Int.right;

            if (dir != Vector2Int.zero)
            {
                Vector2Int currentGrid = worldBoard.GetGridPosition(transform.position);
                Vector2Int targetGrid = currentGrid + dir;
                //Debug.Log("Dir" + dir);
                if (worldBoard.IsWalkable(targetGrid))
                    StartCoroutine(MoveToCell(targetGrid));
                else
                {
                    
                    playerData.canMove = false; // Disable further movement if blocked
                }
            }
        
        }
        else
        {
            playerData.canMove = false; // Disable movement if no move points left
            Debug.Log("No move points left, cannot move.");
        }
    }

    private IEnumerator MoveToCell(Vector2Int targetGrid)
    {
        isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(targetGrid.x, targetGrid.y, startPos.z);
        float t = 0f;
        float duration = Vector2.Distance(startPos, endPos) / speed;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / duration);
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
        playerData.movePoints--;
        diceRoller.UpdateUi(playerData.movePoints);
        //Debug.Log("Moved to " + targetGrid + ", remaining move points: " + movePoints);
        //playerData.canMove = false; // Disable further movement until re-enabled
    }
    
    public void AddScore(int points)
    {
        playerData.AddScore(points);
    }

}