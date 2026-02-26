using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CoinPickup : MonoBehaviour
{
    public int value = 1;

    private void Reset()
    {
        // تأكد أن الـ collider يكون trigger بشكل افتراضي
        Collider c = GetComponent<Collider>();
        if (c != null) c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // أضف العملة إلى الـ CurrencyManager
            SoundManager.Instance?.PlayCoin();
            CurrencyManager.Instance?.AddCoins(value);

            // أي تأثير صوتي/بصري عند الالتقاط هنا (اختياري)
            Destroy(gameObject);
        }
    }
}
