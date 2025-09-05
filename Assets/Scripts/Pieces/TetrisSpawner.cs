using UnityEngine;

public class TetrisSpawner : MonoBehaviour
{
    private WorldBoard board;
    private GameObject blockPrefab;
    private Transform spawnPoint;
    public PieceUIFactory pieceUIFactory;
    public void Initialize(WorldBoard board, GameObject blockPrefab)
    {
        this.board = board;
        this.blockPrefab = blockPrefab;
       
    }

    public void SpawnPieceByType(PiecesType? overrideType = null)
    {
        //Debug.Log("Spawning piece at " + spawnPoint.position);
        PiecesType type = overrideType ?? PieceShapes.GetRandomPieceType();
        GameObject pieceGO = new GameObject("Piece_" + type);
        pieceGO.transform.position = spawnPoint.position;
        pieceUIFactory.CreatePiece(type, color: null, instanceName: pieceGO.name);
        var controller = pieceGO.AddComponent<PieceController>();
        controller.Initialize(board);

        foreach (Vector2 cell in PieceShapes.GetShape(type))
        {
            Vector3 pos = pieceGO.transform.position + (Vector3)cell;
            GameObject block = Instantiate(blockPrefab, pos, Quaternion.identity, pieceGO.transform);
            block.GetComponent<Block>()?.Initialize(board);
        }
    }
    public void SetSpawnPoint(Transform spawnPoint)
    {
        this.spawnPoint = spawnPoint;
    }

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