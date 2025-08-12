using UnityEngine;

public class PiecePath : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public enum PathType { L, T, J, S, Z, I, O };
    public PathType pathType;
    public PieceBase pieceData;

    private float zCoord;
    private Vector3 offset;
    public GameObject blockPrefab;

    void Start()
    {
        // foreach (Vector2 pos in pieceData.blockPositions)
        // {
        //     Vector3 spawnPos = new Vector3(pos.x, pos.y, 0f);
        //     GameObject block = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
        //     block.transform.parent = this.transform;

        // }
    }
    public void CreatePiece(PieceBase pieceData)
    {
        foreach (Vector2 pos in pieceData.blockPositions)
        {
            Vector3 spawnPos = new Vector3(pos.x, pos.y, 0f);
            GameObject block = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
            block.transform.parent = this.transform;
        }
        this.pieceData = pieceData;
        pathType = pieceData.pieceName[0] switch
        {
            'L' => PathType.L,
            'T' => PathType.T,
            'J' => PathType.J,
            'S' => PathType.S,
            'Z' => PathType.Z,
            'I' => PathType.I,
            'O' => PathType.O,
            _ => PathType.L
        };
    }
    // Update is called once per frame
    void Update()
    {

    }
    
    //     void OnMouseDown()
    // {
    //     // Guardamos la coordenada Z en pantalla del objeto
    //     zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

    //     // Calculamos el offset entre la posición del objeto y la del mouse
    //     offset = transform.position - GetMouseWorldPos();
    // }

    // void OnMouseDrag()
    // {
    //     // Mientras mantenemos presionado, actualizamos la posición
    //     transform.position = GetMouseWorldPos() + offset;
    // }

    // // Convierte la posición del mouse en pantalla a posición en mundo
    // private Vector3 GetMouseWorldPos()
    // {
    //     Vector3 mousePoint = Input.mousePosition;
    //     mousePoint.z = zCoord;
    //     return Camera.main.ScreenToWorldPoint(mousePoint);
    // }

}
