using UnityEngine;

public class MovePiece : MonoBehaviour
{
    private float zCoord;
    private Vector3 offset;
    private Transform parentTransform; 
    private WorldBoard worldBoard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parentTransform = transform.parent;
        worldBoard = FindFirstObjectByType<WorldBoard>();
        //Debug.Log("Parent Transform: " + parentTransform);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDown()
    {
        // Guardamos la coordenada Z en pantalla del objeto
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        // Calculamos el offset entre la posición del objeto y la del mouse
        offset = transform.position - GetMouseWorldPos();
        //Debug.Log("Mouse Down: " + transform.position + " Offset: " + offset);
    }

    void OnMouseDrag()
    {
        // Mientras mantenemos presionado, actualizamos la posición
        parentTransform.position = GetMouseWorldPos() + offset;
        //Debug.Log("Mouse Drag: " + transform.position);
    }

    // Convierte la posición del mouse en pantalla a posición en mundo
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        //Debug.Log("Mouse World Pos: " + mousePoint);
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    void OnMouseUp()
    {
        //Debug.Log("Mouse Up: " + transform.position);
        //TetrisSpawner spawner = gameObject.GetComponent<TetrisSpawner>();
        Block block = gameObject.GetComponent<Block>();
        //worldBoard.SetBoard(parentTransform.gameObject);
        //spawner.piece.Setup(pieceName, pieceColor, pieceSprite, pieceValue, pieceType, blockPositions);
    }
}
