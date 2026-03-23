using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int value = 1;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;

    private void Update()
    {
        // Rotate on Y axis (for 3D) — change to Z if you're using 2D sprites
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
    void OnTriggerEnter(Collider other)
    
    {
        if (other.CompareTag("Player"))
        {
            // 🎵 Play coin sound
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayCoin();

            // 💰 Add coins
            if (CoinManager.Instance != null)
                CoinManager.Instance.AddCoins(value);

            // ❌ Destroy coin
           Destroy(gameObject, 0.05f);
        }
    }
}