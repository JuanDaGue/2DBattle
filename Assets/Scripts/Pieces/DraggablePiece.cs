using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    
    // Reference to the 3D GameObject that follows this UI element
    public GameObject worldObject;
    public Camera gameCamera; // Camera that views the 3D world

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            
        // If no camera assigned, try to find main camera
        if (gameCamera == null)
            gameCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        transform.SetAsLastSibling();
        
        // Activate the world object when starting to drag
        if (worldObject != null)
            worldObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        
        // Update the world object position
        UpdateWorldObjectPosition();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        // Optional: Hide or keep the world object visible after drag
        // if (worldObject != null) worldObject.SetActive(false);
    }
    
    // Method to update the 3D GameObject position based on UI position
    private void UpdateWorldObjectPosition()
    {
        if (worldObject == null || gameCamera == null) return;
        
        // Convert UI position to world position
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, rectTransform.position);
        
        // Create a ray from the camera through the UI position
        Ray ray = gameCamera.ScreenPointToRay(screenPoint);
        
        // Define a plane where you want the object to appear (e.g., y=0 for ground)
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        // Find where the ray intersects the plane
        float enter;
        if (groundPlane.Raycast(ray, out enter))
        {
            Vector3 worldPosition = ray.GetPoint(enter);
            worldObject.transform.position = worldPosition;
        }
    }
    
    // Call this method when you want to sync the world object manually
    public void SyncWorldObject()
    {
        UpdateWorldObjectPosition();
    }
}