using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    public PieceBase pieceData; // Renamed for clarity
    public Transform pieceSpawnPoint;
    public WorldBoard worldBoard; // Reference via Inspector
    public PiecesType pieceType; // Type of the piece to spawn
    public GameObject blockPrefab;

    void Start()
    {
        pieceType = PieceShapes.GetRandomPieceType();
        // If pieceData is not assigned, use the type to get the shape
            SpawnPieceByType();
        

        // if (pieceData != null && pieceSpawnPoint != null)
        // {
        //     SpawnPiece();
        // }
    }

    // private void SpawnPiece()
    // {
    //     GameObject pieceObject = new GameObject(pieceData.pieceName);
    //     pieceObject.transform.position = pieceSpawnPoint.position;
    //     PieceController pieceController = pieceObject.AddComponent<PieceController>();
    //     pieceController.Initialize(worldBoard);

    //     foreach (Vector2 pos in pieceData.blockPositions)
    //     {
    //         Vector3 spawnPos = new Vector3(pos.x, pos.y, 0f);
    //         GameObject block = Instantiate(pieceData.blockPrefab,
    //                                        pieceObject.transform.position + spawnPos,
    //                                        Quaternion.identity,
    //                                        pieceObject.transform);

    //         Block blockScript = block.GetComponent<Block>();
    //         if (blockScript != null)
    //         {
    //             blockScript.Initialize(worldBoard);
    //         }
    //     }
    // }
    private void SpawnPieceByType()
    {
        GameObject pieceObject = new GameObject("Piece_" + pieceType);
        pieceObject.transform.position = pieceSpawnPoint.position;
        PieceController pieceController = pieceObject.AddComponent<PieceController>();
        pieceController.Initialize(worldBoard);

        foreach (Vector2 pos in PieceShapes.GetShape(pieceType))
        {
            Vector3 spawnPos = new Vector3(pos.x, pos.y, 0f);
            GameObject block = Instantiate(blockPrefab,
                                           pieceObject.transform.position + spawnPos,
                                           Quaternion.identity,
                                           pieceObject.transform);

            Block blockScript = block.GetComponent<Block>();
            if (blockScript != null)
            {
                blockScript.Initialize(worldBoard);
            }
        }
    }
}