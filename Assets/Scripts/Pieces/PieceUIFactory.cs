using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceUIFactory : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform piecesUI;
    public List<RectTransform> piecesUIList = new List<RectTransform>(2);
    public RectTransform blockPrefab;        // Prefab must be a UI Image (RectTransform)

    [Header("World Object")]
    public GameObject worldObjectPrefab;     // Prefab for the 3D object that follows the UI
    public Camera gameCamera;                // Camera that views the 3D world

    [Header("Layout")]
    public float blockSize = 50f;            // size in pixels for each block
    public Vector2 spacing = Vector2.zero;   // additional spacing on x and y (pixels)

    [Header("Auto Options")]
    public bool clearBeforeCreate = true;    // clear old children before creating new piece

    /// <summary>
    /// Create a piece of the given type centered inside piecesUI.
    /// Optional color will be applied to the Image component of each block.
    /// </summary>
    public RectTransform CreatePiece(PiecesType type, Color? color = null, string instanceName = null)
    {
        if (piecesUI == null)
        {
            Debug.LogError("PieceUIFactory: piecesUI is not assigned.");
            return null;
        }
        if (blockPrefab == null)
        {
            Debug.LogError("PieceUIFactory: blockPrefab is not assigned.");
            return null;
        }

        if (clearBeforeCreate) ClearPieces();

        Vector2[] shape = PieceShapes.GetShape(type);
        if (shape == null || shape.Length == 0) return null;

        // compute bounds (min, max) to center the piece
        Vector2 min = shape[0], max = shape[0];
        foreach (var cell in shape)
        {
            if (cell.x < min.x) min.x = cell.x;
            if (cell.y < min.y) min.y = cell.y;
            if (cell.x > max.x) max.x = cell.x;
            if (cell.y > max.y) max.y = cell.y;
        }
        Vector2 center = (min + max) / 2f;

        // Create a container to hold all blocks so we can move/rotate the whole piece easily
        GameObject pieceGO = new GameObject(instanceName ?? $"Piece_{type}", typeof(RectTransform));
        var pieceRect = pieceGO.GetComponent<RectTransform>();
        pieceRect.SetParent(piecesUI, false);
        pieceRect.anchorMin = pieceRect.anchorMax = new Vector2(0.5f, 0.5f);
        pieceRect.pivot = new Vector2(0.5f, 0.5f);
        pieceRect.anchoredPosition = Vector2.zero;
        pieceRect.sizeDelta = Vector2.zero;

        // Add draggable component
        DraggablePiece draggable = pieceGO.AddComponent<DraggablePiece>();
        draggable.gameCamera = gameCamera;

        // Create world object if prefab is provided
        if (worldObjectPrefab != null)
        {
            GameObject worldObj = Instantiate(worldObjectPrefab);
            worldObj.name = $"{instanceName ?? type.ToString()}_WorldObject";
            worldObj.SetActive(false); // Hide initially

            // Link the world object to the UI element
            draggable.worldObject = worldObj;
        }

        // Instantiate blocks
        float stepX = blockSize + spacing.x;
        float stepY = blockSize + spacing.y;

        foreach (var cell in shape)
        {
            RectTransform block = Instantiate(blockPrefab, pieceRect);
            block.name = $"block_{cell.x}_{cell.y}";
            block.SetAsLastSibling();

            // ensure block has correct size
            block.sizeDelta = new Vector2(blockSize, blockSize);

            // anchoredPosition relative to pieceRect; center the piece using center offset
            Vector2 anchored = (cell - center);
            Vector2 anchoredPixels = new Vector2(anchored.x * stepX, anchored.y * stepY);
            block.anchoredPosition = anchoredPixels;

            // apply color if provided
            var img = block.GetComponent<Image>();
            if (img != null && color.HasValue)
            {
                img.color = color.Value;
            }
        }

        return pieceRect;
    }

    /// <summary>
    /// Removes all child objects from the piecesUI container.
    /// </summary>
    public void ClearPieces()
    {
        if (piecesUI == null) return;
        for (int i = piecesUI.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(piecesUI.GetChild(i).gameObject);
        }
    }
    public void SetcurrentPiece(int index)
    {
        if (index < 0 || index >= piecesUIList.Count) return;
        piecesUI = piecesUIList[index];
    }
    // ... rest of your existing methods ...
}
