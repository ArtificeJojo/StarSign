using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour
{
    [Header("Tracking Targets")]
    public Transform playerTransform;

    [Header("Position Offsets")]
    public Vector3 offset = new Vector3(0f, 2f, -5f); // Hover height and distance behind
    public float smoothSpeed = 10f;

    [Header("Mouse Orbit Settings")]
    public float sensitivityX = 3f;
    public float sensitivityY = 3f;
    public float minYAngle = -20f; // Limit looking too far down
    public float maxYAngle = 60f;  // Limit looking too far up

    private float currentX = 0f;
    private float currentY = 0f;

    void Start()
    {
        // Lock and hide the cursor for seamless mouse rotation
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (playerTransform == null) return;

        // Gather raw mouse inputs multiplied by sensitivity
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

        // Clamp the vertical camera angle so it doesn't flip upside down
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        // Calculate rotation based on mouse movement
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);

        // Calculate target camera position behind the player using the rotation offset
        Vector3 targetPosition = playerTransform.position + (rotation * offset);

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Constantly look back at the player's position
        transform.LookAt(playerTransform.position + Vector3.up * offset.y * 0.5f);
    }
}

