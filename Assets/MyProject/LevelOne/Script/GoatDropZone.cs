using UnityEngine;

public class GoatDropZone : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered Goat Drop Zone");
        }
    }
}
