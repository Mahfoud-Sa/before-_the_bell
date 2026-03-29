using UnityEngine;

public interface ICollectible
{
    void OnCollected();
}

public class CollectItemScript : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;
    public bool use2D = false; 

    private void Update()
    {
        float step = rotationSpeed * Time.deltaTime;
        if (use2D)
            transform.Rotate(0f, 0f, step);
        else
            transform.Rotate(0f, step, 0f);
    }

    // --- 3D Collision ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HandleCollection();
        }
    }

    // --- 2D Collision (For Sprites) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandleCollection();
        }
    }

    private void HandleCollection()
    {
        // ✅ Notify ToDoManager
        if (ToDoManagementScript.Instance != null)
        {
            // We pass this specific GameObject so the Manager knows which checkmark to enable
            ToDoManagementScript.Instance.CollectItem(gameObject);
        }

        // ❌ Remove item from scene
        // We use a tiny delay to ensure the Manager processes the reference before it's destroyed
        Destroy(gameObject, 0.02f);
    }
}