using UnityEngine;

public class GoatTrigger : MonoBehaviour
{
    public GoatPickup goat;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            goat.SetPlayerTouching(true);
            SoundManager.Instance.PlayGoat();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            goat.SetPlayerTouching(false);
        }
    }
}