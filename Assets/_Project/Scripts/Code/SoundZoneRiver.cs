using UnityEngine;

public class SoundZoneRiver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered SoundZone");
            SoundManager.Instance.PlayRiverSound();
            //SoundManager.Instance.PlayGotsSound();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Left SoundZone");
            SoundManager.Instance.StopRiverSound();
        }
    }
}
