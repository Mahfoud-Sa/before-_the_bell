using UnityEngine;

public class MudAreaZoneScript : MonoBehaviour
{
  
    [Header("Mud Area Settings")]
    [SerializeField] private float areaSpeed = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement1.instance.EnterMudArea(areaSpeed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement1.instance.ExitMudArea();
        }
    }

}
