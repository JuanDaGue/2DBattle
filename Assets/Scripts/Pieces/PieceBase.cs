using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "PieceBase", menuName = "PieceBase/Create New Piece")]
public class PieceBase : ScriptableObject
{
    // This class serves as a base for all pieces in the game.
    // It can be extended to include properties and methods common to all pieces.
    public Color pieceColor; // Color of the piece
    public string pieceName; // Name of the piece
    public Sprite pieceSprite; // Visual representation of the piece
    public int pieceValue; // Value of the piece, used for scoring or other purposes
    public GameObject blockPrefab; // Prefab for the blocks that make up the piece
    // You can add more properties and methods as needed for your game logic
    public PiecesType pieceType; // Type of the piece (I, O, L, T, J, S, Z)

    public Vector2[] blockPositions; // Positions of the blocks on the board
    // Example method to display piece information
    public void DisplayInfo()
    {
        Debug.Log($"Piece Name: {pieceName}, Value: {pieceValue}");
    }

    public void Setup(string pieceName_, Color pieceColor_, Sprite pieceSprite_, int pieceValue_, PiecesType pieceType_, Vector2[] blockPositions_)
    {
        pieceName = pieceName_;
        pieceColor = pieceColor_;
        pieceSprite = pieceSprite_;
        pieceValue = pieceValue_;
        pieceType = pieceType_;
        blockPositions = blockPositions_;
    }
    // public void DragPiece(Vector2 mousePos, int i)  
    // {
    //     Vector2 offset = mousePos - new Vector2(blockPositions[i].x, blockPositions[i].y);
    //     for (int j = 0; j < blockPositions.Length; j++)
    //     {
    //         blockPositions[j] += offset;
    //     }

    // }   
}

