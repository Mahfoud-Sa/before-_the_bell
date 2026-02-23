using UnityEngine;

public class HoleScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
       private void Awake()
    {
        Debug.Log($"[KillZone] Script attached to: {gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[KillZone] Trigger entered by: {other.name}");

        // Check tag
        Debug.Log($"[KillZone] Tag detected: {other.tag}");

        // Check Rigidbody
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null)
        {
            Debug.LogWarning($"[KillZone] {other.name} has NO Rigidbody!");
        }
        else
        {
            Debug.Log($"[KillZone] Rigidbody found on {other.name}");
        }

        // Check Collider
        Collider col = other.GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogWarning($"[KillZone] {other.name} has NO Collider!");
        }

        // Player check
        if (!other.CompareTag("Player"))
        {
            Debug.LogWarning($"[KillZone] {other.name} is NOT tagged as Player");
            return;
        }

        Debug.Log("☠️ PLAYER KILLED ☠️");

        // Kill action
        other.gameObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"[KillZone] Staying inside trigger: {other.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[KillZone] Exited trigger: {other.name}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
