using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int x, y;
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public float maxDistance = 3f;
    private Vector2 startPosition;
    private WorldBoard worldBoard;

    public void Initialize(WorldBoard board)
    {
        worldBoard = board;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * speed;
    }


    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
    }
    void PlayerInputs()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W key pressed");
            y++;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S key pressed");
            y--;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A key pressed");
            x--;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D key pressed");
            x++;
        }
        else
        {
            movement = Vector2.zero;
        }
        //updateGridPosition();
        Move();
    
        // rb.linearVelocity = movement;
        // transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        // if (transform.position.magnitude > maxDistance)
        // {
        //     transform.position = startPosition;
        //     rb.linearVelocity = Vector2.zero;
        // }
    }
    public void updateGridPosition()
    {
        x = Mathf.RoundToInt(transform.position.x);
        y = Mathf.RoundToInt(transform.position.y);
    }

    private void Move()
    {
        // Example movement logic: update position based on x, y and speed
        Vector2 direction = new Vector2(x, y);
        movement = direction * speed;
        //rb.linearVelocity = movement;
        transform.position = Vector2.MoveTowards(transform.position, direction, speed * Time.deltaTime);
        if (transform.position.magnitude > maxDistance)
        {
            //transform.position = startPosition;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
