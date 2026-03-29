using UnityEngine;

public class FloatingPlatformScript : MonoBehaviour
{
    public enum MoveDirection { X, Y, Z }

    [Header("Movement Settings")]
    public MoveDirection direction = MoveDirection.X;
    public float moveDistance = 5f;
    public float speed = 2f;

    private Vector3 startPos;
    private Vector3 endPos;
    private bool movingToEnd = true;

    private void Start()
    {
        startPos = transform.position;
        
        // Determine the target vector based on the chosen direction
        Vector3 directionVector = Vector3.zero;
        switch (direction)
        {
            case MoveDirection.X: directionVector = transform.right; break;
            case MoveDirection.Y: directionVector = transform.up; break;
            case MoveDirection.Z: directionVector = transform.forward; break;
        }

        endPos = startPos + directionVector * moveDistance;
    }

    private void Update()
    {
        Vector3 target = movingToEnd ? endPos : startPos;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Check if we reached the target to flip direction
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingToEnd = !movingToEnd;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Makes the player move with the platform
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Releases the player
            collision.transform.SetParent(null);
        }
    }
}