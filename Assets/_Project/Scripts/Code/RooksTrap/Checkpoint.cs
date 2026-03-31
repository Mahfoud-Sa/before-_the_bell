using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;

        if (other.CompareTag("Player"))
        {
            isActivated = true;

            if (CheckpointManager.Instance != null)
            {
                CheckpointManager.Instance.SetCheckpoint(transform);
            }

            Debug.Log("[Checkpoint] Activated: " + gameObject.name);

            // Optional: play sound
            // SoundManager.Instance?.PlayCheckpointSound();
        }
    }
}