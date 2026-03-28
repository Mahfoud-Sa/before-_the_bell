using System.Collections;
using UnityEngine;

public class clearSpotScript : MonoBehaviour
{
    [Header("Clean Strength")]
    [SerializeField] private float valueToIncrease = 1f;

    [Header("Behavior")]
    [SerializeField] private bool destroyAfterUse = false;
    [SerializeField] private float hitCooldown = 1f;

    private bool canHit = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canHit) return;

        if (other.CompareTag("Player"))
        {
            // Reverse mud effect (restore player stats)
            PlayerMovement1.CleanMud(valueToIncrease);

            if (destroyAfterUse)
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(HitDelay());
            }
        }
    }

    private IEnumerator HitDelay()
    {
        canHit = false;
        yield return new WaitForSeconds(hitCooldown);
        canHit = true;
    }
}