using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private WorldBoard worldBoard;

    [SerializeField] private bool isMoving = false;
    private Player playerData;

    void Update()
    {
        if (isMoving) return;
        HandleInput();
    }
    public void Initialize(Player data, WorldBoard board)
    {
        playerData = data;
        worldBoard = board;
        // Optionally use playerData to set initial state
        Debug.Log($"Initialized movement for {playerData.PlayerName}");
    }


    private void HandleInput()
    {
        //Debug.Log("Handling input for movement"+ isMoving);
        Vector2Int dir = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W)) dir = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S)) dir = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A)) dir = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D)) dir = Vector2Int.right;

        if (dir != Vector2Int.zero)
        {
            Vector2Int currentGrid = worldBoard.GetGridPosition(transform.position);
            Vector2Int targetGrid = currentGrid + dir;

            if (worldBoard.IsWalkable(targetGrid))
                StartCoroutine(MoveToCell(targetGrid));
            else
                Debug.Log("Blocked at " + targetGrid);
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
    }
    
    public void AddScore(int points)
    {
        playerData.AddScore(points);
    }

}