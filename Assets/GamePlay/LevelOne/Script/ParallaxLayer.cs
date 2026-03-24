using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private float speed = 2f; // units per second
    [SerializeField] private bool moveRight = false; // Direction of movement

    private float layerWidth;       // Width of the plane
    private Vector3 startPos;       // Starting position
    private Vector3 resetPos;       // Position to reset to

    void Start()
    {
        // Calculate the width of the plane
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
            layerWidth = mr.bounds.size.x;
        else
            layerWidth = 1f; // fallback

        startPos = transform.position;

        // Set reset position depending on direction
        resetPos = moveRight 
            ? new Vector3(startPos.x + layerWidth, startPos.y, startPos.z) 
            : new Vector3(startPos.x - layerWidth, startPos.y, startPos.z);

        // Optional: Snap initial position to startPos
        transform.position = startPos;
    }

    void Update()
    {
        float direction = moveRight ? 1f : -1f;

        // Move the layer
        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);

        // Check for reset
        if (!moveRight && transform.position.x <= resetPos.x)
        {
            transform.position = startPos;
        }
        else if (moveRight && transform.position.x >= resetPos.x)
        {
            transform.position = startPos;
        }
    }
}