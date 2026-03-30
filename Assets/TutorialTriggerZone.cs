using UnityEngine;

public class TutorialTriggerZone : MonoBehaviour
{
    public bool triggerOnce = true;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
       // ShowMessage("Before going to school, complete these tasks");
        if (hasTriggered && triggerOnce) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (TutorialManager.Instance != null)
            {
                TutorialManager.Instance.StartTutorialFromTrigger();
            }
        }
    }
    
}