using System.Collections;
using UnityEngine;

public class MudAreaScript : MonoBehaviour
{
    [Header("Mud Strength")]
    [SerializeField] private float valueToDecrease = 1f;

    [Header("Behavior")]
    [SerializeField] private bool destroyAfterHit = false;
    [SerializeField] private float hitCooldown = 1f;

    private bool canHit = true;

    private void OnTriggerEnter(Collider other)
    {
       // if (!canHit) return;

        if (other.CompareTag("Player"))
        {
            PlayerMovement1.HitMud(valueToDecrease,0.2f);

            if (destroyAfterHit)
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