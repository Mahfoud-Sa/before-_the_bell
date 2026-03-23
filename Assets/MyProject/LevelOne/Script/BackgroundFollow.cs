using UnityEngine;

[ExecuteAlways]
public class BackgroundFollowAndCover : MonoBehaviour
{


    [SerializeField] private Transform targetCamera; // Camera to follow
    [SerializeField] private Vector3 offset = Vector3.zero; // Optional offset on X
    [SerializeField, Range(0.01f, 1f)] private float smoothSpeed = 0.1f; // 0 = slow, 1 = instant

    void Start()
    {
        if (!targetCamera)
            targetCamera = Camera.main.transform; // Use main camera if none assigned
    }

    void LateUpdate()
    {
        if (!targetCamera) return;

        // Target X position with offset
        float targetX = targetCamera.position.x + offset.x;

        // Smoothly interpolate current X to target X
        float smoothX = Mathf.Lerp(transform.position.x, targetX, smoothSpeed);

        // Apply new position, keep Y and Z unchanged
        transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);
    
}
}