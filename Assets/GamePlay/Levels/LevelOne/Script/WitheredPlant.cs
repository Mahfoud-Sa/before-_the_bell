using UnityEngine;

public class WitheredPlant : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The tool required to water the plant")]
    public string requiredToolName = "FullGardel";
    
    [Header("Visual Replacements")]
    [Tooltip("The green plant sprite (for 2D)")]
    public Sprite wateredPlantSprite;
    [Tooltip("The green plant material (for 3D)")]
    public Material wateredPlantMaterial;
    
    [Header("Optional Effects")]
    [Tooltip("The visual effect when watered")]
    public GameObject wateringEffect;
    
    private bool isWatered = false;
    private SpriteRenderer spriteRenderer;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isWatered && other.CompareTag("Player"))
        {
            CheckAndWaterPlant();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isWatered && collision.gameObject.CompareTag("Player"))
        {
            CheckAndWaterPlant();
        }
    }

    private void CheckAndWaterPlant()
    {
        if (AdvancedToolManager.currentToolName == requiredToolName)
        {
            if (AdvancedToolManager.Instance != null && AdvancedToolManager.Instance.isGardelFull)
            {
                WaterPlant();
            }
        }
    }

    private void WaterPlant()
    {
        isWatered = true;
        
        if (spriteRenderer != null && wateredPlantSprite != null)
        {
            spriteRenderer.sprite = wateredPlantSprite;
        }
        else if (meshRenderer != null && wateredPlantMaterial != null)
        {
            meshRenderer.material = wateredPlantMaterial;
        }

        if (wateringEffect != null)
        {
            Instantiate(wateringEffect, transform.position, Quaternion.identity);
        }

        AdvancedToolManager.Instance.ResetGardelToEmpty();
        
        Debug.Log("Plant watered and turned green!");
    }
}
