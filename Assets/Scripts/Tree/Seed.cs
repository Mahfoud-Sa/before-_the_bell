using UnityEngine;
using System.Collections;

public class Seed : MonoBehaviour
{
    [HideInInspector] 
    public GameObject originalTree; 

    [Header("Animation Settings")]
    public float dropDuration = 0.5f;   
    public float spreadDistance = 1.0f; 
    public float jumpHeight = 1.5f;     

    void Start()
    {
        StartCoroutine(PopOutAnimation());
    }

    IEnumerator PopOutAnimation()
    {
        Vector3 startPos = transform.position;
        Vector3 randomOffset = new Vector3(Random.Range(-spreadDistance, spreadDistance), Random.Range(-spreadDistance, spreadDistance), 0);
        Vector3 targetPos = startPos + randomOffset;

        float elapsed = 0f;

        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dropDuration;
            
            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight; 
            
            transform.position = Vector3.Lerp(startPos, targetPos, t) + new Vector3(0, yOffset, 0);
            yield return null;
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            string tool = AdvancedToolManager.currentToolName;
            playerMove player = FindObjectOfType<playerMove>();

            if (tool == "Gardel")
            {
                if (player != null) player.StartActionAnim();

                if (originalTree != null)
                {
                    TreeScript treeScript = originalTree.GetComponent<TreeScript>();
                    if (treeScript != null)
                    {
                        treeScript.RestoreTreeAndDestroySeeds();
                    }
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}