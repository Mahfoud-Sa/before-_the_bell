using UnityEngine;
using System.Collections;

public class Seed : MonoBehaviour
{
    [HideInInspector] 
    public GameObject originalTree; 

    [Header("Animation Settings")]
    public float dropDuration = 0.6f;     
    public float jumpHeight = 1.5f;        
    public float rotationSpeed = 180f;    

    [HideInInspector]
    public int index = 0;                 
    [HideInInspector]
    public int totalSeeds = 1;            

    private Vector3 targetPos;

    void Start()
    {
        CalculateTargetPosition();
        StartCoroutine(PopOutAnimation());
    }

    void CalculateTargetPosition()
    {
        // توزيع البذور حول الشجرة في دائرة (يمكن التحكم في نصف القطر من TreeScript)
        if (originalTree == null) return;

        float angle = 360f / totalSeeds * index;
        float rad = angle * Mathf.Deg2Rad;
        float radius = (originalTree.GetComponent<TreeScript>() != null) ? 
                        originalTree.GetComponent<TreeScript>().seedRadius : 0.7f;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * radius;

        targetPos = originalTree.transform.position + offset;
        targetPos.z = 0f; // ثابت Z
    }

    IEnumerator PopOutAnimation()
    {
        Vector3 startPos = originalTree != null ? originalTree.transform.position : transform.position;
        startPos.z = 0f;
        transform.position = startPos;

        float elapsed = 0f;

        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dropDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector3 horizontalMove = Vector3.Lerp(startPos, targetPos, smoothT);
            float height = 4f * jumpHeight * smoothT * (1f - smoothT);

            transform.position = new Vector3(horizontalMove.x, horizontalMove.y + height, 0f);

            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime, Space.Self);

            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = Quaternion.identity;
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
                        treeScript.RestoreTreeAndDestroySeeds();
                }

                Destroy(gameObject);
            }
        }
    }
}