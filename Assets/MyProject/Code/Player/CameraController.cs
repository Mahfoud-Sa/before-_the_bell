using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    // Adjust these values in the Inspector
    [SerializeField] private Vector3 offset = new Vector3(4f, 2f, 0f);

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + offset;
        targetPosition.z = transform.position.z; // Keep original Z

        transform.position = targetPosition;
    }
}