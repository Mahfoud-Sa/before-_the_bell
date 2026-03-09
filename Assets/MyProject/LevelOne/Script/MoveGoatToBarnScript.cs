using UnityEngine;
using System.Collections;

public class MoveGoatToBarnSimple : MonoBehaviour
{
    [Header("Settings")]
    public GameObject player;
    public Transform finalGoatPosition;
    public float moveSpeed = 5f;

    [Header("Optional")]
    public GameObject goat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player) return;

        // Try to detect goat if not assigned
        if (goat == null)
        {
            Transform heldGoat = other.transform.Find("Goat");
            if (heldGoat != null)
                goat = heldGoat.gameObject;
        }

        if (goat != null)
        {
            StartCoroutine(MoveGoatCoroutine(goat, finalGoatPosition.position));
        }
        else
        {
            Debug.LogWarning("No goat found to move!");
        }
    }

    private IEnumerator MoveGoatCoroutine(GameObject goatObj, Vector3 targetPos)
    {
        // 1️⃣ Stop Goat AI if it exists
        GoatWanderAI ai = goatObj.GetComponent<GoatWanderAI>();
        if (ai != null)
            ai.enabled = false;

        // 2️⃣ Detach from player
        goatObj.transform.SetParent(null);

        // 3️⃣ Disable physics while moving
        Rigidbody rb = goatObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Move goat to barn
        while (Vector3.Distance(goatObj.transform.position, targetPos) > 0.05f)
        {
            goatObj.transform.position = Vector3.MoveTowards(
                goatObj.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Snap to final position
        goatObj.transform.position = targetPos;
        goatObj.transform.rotation = Quaternion.identity;

        Debug.Log("🐐 Goat delivered to barn!");
    }
}