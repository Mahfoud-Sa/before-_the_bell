using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Death Zone");
            SoundManager.Instance.PlayPlayerHit();
            CheckpointManager.Instance.Respawn();
        }
    }
}

