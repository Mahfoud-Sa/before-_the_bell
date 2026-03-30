using UnityEngine;
using System.Collections;

public class PalmBridge : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasFallen = false;
    private bool isPlayerTouching = false;

    // 🔥 النخلة الحالية التي يلمسها اللاعب
    public static PalmBridge currentPalm;

    [Header("Tool Settings")]
    public string requiredToolName = "Axe";

    [Header("Wood Spawn Settings")]
    public GameObject woodLogPrefab;
    public int woodCount = 3;
    public float delayBeforeTransform = 3f;

    [Header("VFX Settings")]
    public GameObject dustEffectPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = true;
    }

   public void OnAxeButtonPressed()
{
     StartFalling();
    // Debug.Log("Trying to cut tree: " + name);

    // if (currentPalm != this)
    // {
    //     Debug.Log("Not current palm");
    //     return;
    // }

    // if (!isPlayerTouching)
    // {
    //     Debug.Log("Player not touching");
    //     return;
    // }

    // if (hasFallen)
    // {
    //     Debug.Log("Already fallen");
    //     return;
    // }

    // if (AdvancedToolManager.currentToolName != requiredToolName)
    // {
    //     Debug.Log("Wrong tool: " + AdvancedToolManager.currentToolName);
    //     return;
    // }

    // Debug.Log("Tree will fall now!");
    // StartFalling();
}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = true;
            currentPalm = this; // 🔥 تسجيل هذه النخلة كالنخلة الحالية
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = false;

            if (currentPalm == this)
                currentPalm = null;
        }
    }

    void StartFalling()
    {
        hasFallen = true;

        rb.isKinematic = false;
        rb.useGravity = true;

        rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        Vector3 fallAxis = Vector3.Cross(transform.forward, Vector3.up).normalized;
        rb.AddTorque(fallAxis * 30f, ForceMode.Impulse);

        Debug.Log("Tree falling... will turn to wood in " + delayBeforeTransform + " seconds");

        StartCoroutine(ConvertToWood());
    }

    IEnumerator ConvertToWood()
    {
        yield return new WaitForSeconds(delayBeforeTransform);

        // 🪵 إنشاء الحطب
        for (int i = 0; i < woodCount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(
                Random.Range(-0.5f, 0.5f),
                i * 0.5f,
                Random.Range(-0.5f, 0.5f)
            );

            Instantiate(woodLogPrefab, spawnPos, Random.rotation);
        }

        // 💨 تأثير الغبار
        if (dustEffectPrefab != null)
        {
            Instantiate(dustEffectPrefab, transform.position, Quaternion.identity);
        }

        // ❌ حذف النخلة
        Destroy(gameObject);
    }
}