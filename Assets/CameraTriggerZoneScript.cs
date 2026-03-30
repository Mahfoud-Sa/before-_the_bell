using UnityEngine;

public class CameraTriggerZoneScript : MonoBehaviour
{
   
   
    [SerializeField] private CameraController cameraController;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        cameraController.LockCamera();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        cameraController.UnlockCamera();
    }

}
