using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.Instance.SetCheckpoint(transform);
        }
    }
}