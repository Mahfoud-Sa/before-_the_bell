using UnityEngine;

public class SpawnZoneTrigger : MonoBehaviour
{
    public RockSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Zone");
            SoundManager.Instance.PlayComingRockSound();
            spawner.StartSpawning(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Left Zone");
            spawner.StopSpawning();
            spawner.ClearAllRocks();
        }
    }
}
