using UnityEngine;

public class StoneWaterMovment : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatSpeed = 1f;     // Speed of up/down movement
    public float floatHeight = 0.5f;  // Height of movement
    public bool reverse = false;      // Reverse movement phase

    [Header("Rotation Settings")]
    public bool enableRotation = false;
    public float rotationSpeed = 50f; // Y-axis rotation speed

    private Vector3 startPos;
    private float offset;

    void Start()
    {
        startPos = transform.position;

        // If reverse is ON, shift the sine wave by PI (180 degrees)
        offset = reverse ? Mathf.PI : 0f;
    }

    void Update()
    {
        // Floating movement
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed + offset) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Optional rotation
        if (enableRotation)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }
}