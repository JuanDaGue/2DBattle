using UnityEngine;
using UnityEngine.UI;

public class PieceUIFactory : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform piecesUI;           // Parent container inside Canvas
    public RectTransform blockPrefab;        // Prefab must be a UI Image (RectTransform)

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
        Button pieceButton = pieceGO.AddComponent<Button>();
        pieceButton.onClick.AddListener(() => OnPieceClicked());
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
    /// Create piece and place the group at a custom anchoredPosition inside the piecesUI.
    /// piecesUI must be the parent. anchoredPos is in local pixels relative to piecesUI pivot.
    /// </summary>
    public RectTransform CreatePieceAt(PiecesType type, Vector2 anchoredPos, Color? color = null, string instanceName = null)
    {
        var piece = CreatePiece(type, color, instanceName);
        if (piece != null) piece.anchoredPosition = anchoredPos;
        return piece;
    }

    /// <summary>
    /// Destroy all children inside piecesUI (useful to clear previous piece).
    /// </summary>
    public void ClearPieces()
    {
        if (piecesUI == null) return;
        for (int i = piecesUI.childCount - 1; i >= 0; --i)
        {
            var child = piecesUI.GetChild(i);
            Destroy(child.gameObject);
        }
    }
    
      private void OnPieceClicked()
    {
        Debug.Log("Dragging");
    }
}
