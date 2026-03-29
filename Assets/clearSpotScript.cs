using System.Collections;
using UnityEngine;

public class clearSpotScript : MonoBehaviour
{
    [Header("Clean Settings")]
    [SerializeField] private float valueToIncrease = 1f;
    [SerializeField] private int cleanPrice = 10;

    [Header("Behavior")]
    [SerializeField] private bool destroyAfterUse = false;
    [SerializeField] private float hitCooldown = 1f;

    [Header("UI")]
    [SerializeField] private GameObject cleanButtonUI;

    private bool canUse = true;
    private bool playerInside = false;

    private void Start()
    {
        if (cleanButtonUI != null)
            cleanButtonUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (cleanButtonUI != null)
                cleanButtonUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (cleanButtonUI != null)
                cleanButtonUI.SetActive(false);
        }
    }

    // هذا يتم استدعاؤه من زر UI
    public void TryClean()
    {
        if (!playerInside || !canUse) return;

         if (CoinManager.Instance.currentCoins >= cleanPrice)
        // {
        //     // خصم المال
            CoinManager.Instance.currentCoins -= cleanPrice;

        //     // تنظيف الطين (عكس التأثير)
             PlayerMovement1.CleanMud(valueToIncrease);

        //     if (destroyAfterUse)
        //     {
        //         Destroy(gameObject);
        //     }
        //     else
        //     {
        //         StartCoroutine(Cooldown());
        //     }
        // }
        // else
        // {
        //     Debug.Log("Not enough money!");
        // }
    }

    private IEnumerator Cooldown()
    {
        canUse = false;

        if (cleanButtonUI != null)
            cleanButtonUI.SetActive(false);

        yield return new WaitForSeconds(hitCooldown);

        canUse = true;

        if (playerInside && cleanButtonUI != null)
            cleanButtonUI.SetActive(true);
    }

    // للحصول على السعر في UI (اختياري)
    public int GetPrice()
    {
        return cleanPrice;
    }
}