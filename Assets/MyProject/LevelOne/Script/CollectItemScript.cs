using UnityEngine;

public class CollectItemScript : MonoBehaviour
{
   [Header("Rotation Settings")]
    public float rotationSpeed = 200f;
    public bool use2D = false; // 👈 if you're using sprites

    private void Update()
    {
        if (use2D)
        {
            // Rotate for 2D (Z axis)
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Rotate for 3D (Y axis)
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 🎵 Optional sound
           // if (SoundManager.Instance != null)
              //  SoundManager.Instance.PlayCoin();

            // ✅ Notify ToDoManager
            if (ToDoManagmentScript.Instance != null)
                ToDoManagmentScript.Instance.CollectItem(gameObject);

            // ❌ Remove item
            Destroy(gameObject, 0.05f);
        }
    }
}
