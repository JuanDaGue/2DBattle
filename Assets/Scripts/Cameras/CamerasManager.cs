using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CamerasManager : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera mainCamera;
    public List<Transform> cameraTargets = new List<Transform>();
    public float cameraSmoothSpeed = 5f;
    private GameManager gameManager;

    public float zoomSpeed = 2f;
    public float offsetX = 0f;
    public float offsetY = 15f;
    
    // Camera configuration parameters
    private Vector2 farCameraPosition = new Vector2(15f, 25f);
    private float farCameraSize = 30f;
    
    private Vector2 middleCameraMinPosition = new Vector2(5f, 5f);
    private Vector2 middleCameraMaxPosition = new Vector2(25f, 45f);
    private float middleCameraSize = 10f;
    
    private Vector2 nearCameraMinPosition = new Vector2(0f, 0f);
    private Vector2 nearCameraMaxPosition = new Vector2(30f, 50f);
    private float nearCameraSize = 5f;
    
    private Transform currentTarget;
    private CameraMode currentCameraMode = CameraMode.Middle;
    private float aux=1f;
    
    // Enum to track current camera mode
    private enum CameraMode
    {
        Far,
        Middle,
        Near
    }

    void Start()
    {
        mainCamera.orthographicSize = middleCameraSize;
        ApplyCameraSettings();
    }
    
    public void InitializeCamera(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    private IEnumerator MoveCameraToTarget(Transform target)
    {
        Vector3 targetPosition = CalculateTargetPosition(target);
        ValidateTargetPosition(ref targetPosition);

        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                cameraSmoothSpeed * Time.deltaTime
            );
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
    }

    private Vector3 CalculateTargetPosition(Transform target)
    {
        if (currentCameraMode == CameraMode.Far)
        {
            return new Vector3(farCameraPosition.x, farCameraPosition.y, mainCamera.transform.position.z);
        }
        else
        {
            return new Vector3(target.position.x + offsetX, target.position.y + offsetY, mainCamera.transform.position.z);
        }
    }

    private void ValidateTargetPosition(ref Vector3 targetPosition)
    {
        if (currentCameraMode == CameraMode.Far) return;
        
        Vector2 minPosition, maxPosition;
        
        if (currentCameraMode == CameraMode.Middle)
        {
            minPosition = middleCameraMinPosition;
            maxPosition = middleCameraMaxPosition;
        }
        else // Near camera
        {
            minPosition = nearCameraMinPosition;
            maxPosition = nearCameraMaxPosition;
        }
        
        targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minPosition.y, maxPosition.y);
    }

    public void SwitchCameraToPlayer(int playerId)
    {
        if (playerId - 1 < cameraTargets.Count && cameraTargets[playerId - 1] != null)
        {
            currentTarget = cameraTargets[playerId - 1];
            StartCoroutine(MoveCameraToTarget(currentTarget));
        }
        if(playerId==1){
            aux=1f;
        }
        else{
            aux=-1f;
        }
        //Debug.Log("aux"+ aux);
    }

    public void Update()
    {
        HandleCameraModeChange();
        UpdateOffset();
        UpdateCameraPosition();
    }
    
    private void HandleCameraModeChange()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll > 0) // Scrolling up - zoom out
        {
            if (currentCameraMode == CameraMode.Near)
            {
                currentCameraMode = CameraMode.Middle;
                ApplyCameraSettings();
            }
            else if (currentCameraMode == CameraMode.Middle)
            {
                currentCameraMode = CameraMode.Far;
                ApplyCameraSettings();
            }
        }
        else if (scroll < 0) // Scrolling down - zoom in
        {
            if (currentCameraMode == CameraMode.Far)
            {
                currentCameraMode = CameraMode.Middle;
                ApplyCameraSettings();
            }
            else if (currentCameraMode == CameraMode.Middle)
            {
                currentCameraMode = CameraMode.Near;
                ApplyCameraSettings();
            }
        }
    }
    
    private void ApplyCameraSettings()
    {
        switch (currentCameraMode)
        {
            case CameraMode.Far:
                mainCamera.orthographicSize = farCameraSize;
                offsetY=0f;
                break;
            case CameraMode.Middle:
                mainCamera.orthographicSize = middleCameraSize;
                offsetY=7.5f*aux;
                break;
            case CameraMode.Near:
                mainCamera.orthographicSize = nearCameraSize;
                offsetY=3.5f*aux;
                break;
        }
    }

    private void UpdateOffset()
    {
        // Only allow offset adjustment in Middle and Near modes
        if (currentCameraMode != CameraMode.Far)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                offsetX += 0.1f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                offsetX -= 0.1f;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                offsetY += 0.1f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                offsetY -= 0.1f;
            }
        }
    }

    private void UpdateCameraPosition()
    {
        if (currentTarget == null) return;

        Vector3 targetPosition = CalculateTargetPosition(currentTarget);
        ValidateTargetPosition(ref targetPosition);

        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            targetPosition,
            cameraSmoothSpeed * Time.deltaTime
        );
    }
}