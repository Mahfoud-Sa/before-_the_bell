using UnityEngine;

public class WaterSource : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The tool required to collect water")]
    public string requiredToolName = "EmptyGardel";
    
    [Header("Optional Effects")]
    [Tooltip("The visual effect when water is collected")]
    public GameObject waterCollectEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CheckAndFillBucket();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CheckAndFillBucket();
        }
    }

    private void CheckAndFillBucket()
    {
        if (AdvancedToolManager.currentToolName == requiredToolName)
        {
            if (AdvancedToolManager.Instance != null && !AdvancedToolManager.Instance.isGardelFull)
            {
                AdvancedToolManager.Instance.FillGardel();
                
                if (waterCollectEffect != null)
                {
                    Instantiate(waterCollectEffect, transform.position, Quaternion.identity);
                }

                Debug.Log("Bucket filled with water!");
            }
        }
    }
}
