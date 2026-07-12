using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    [Header("Tracking Targets")]
    public Transform playerTransform;

    [Header("Position Offsets")]
    public Vector3 offset = new Vector3(0f, 2f, -5f); 
    public float smoothSpeed = 10f;

    [Header("Mouse Orbit Settings")]
    public float sensitivityX = 3f;
    public float sensitivityY = 3f;
    public float minYAngle = -20f; 
    public float maxYAngle = 60f;  

    private float currentX = 0f;
    private float currentY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (playerTransform == null) return;
        
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
        
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);

        Vector3 targetPosition = playerTransform.position + (rotation * offset);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        transform.LookAt(playerTransform.position + Vector3.up * offset.y * 0.5f);
    }
}

