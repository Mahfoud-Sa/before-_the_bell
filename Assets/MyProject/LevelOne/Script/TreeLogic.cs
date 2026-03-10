using UnityEngine;
using System.Collections; // أضفنا هذا السطر لاستخدام الـ Coroutine

public class PalmBridge : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasFallen = false;
    private bool isPlayerTouching = false;

    [Header("Tool Settings")]
    public string requiredToolName = "Axe";

    [Header("Wood Spawn Settings")]
    public GameObject woodLogPrefab; // اسحب بريفاب الحطب (الأزرق) هنا
    public int woodCount = 3;        // عدد قطع الخشب التي ستظهر
    public float delayBeforeTransform = 3f; // الوقت بالثواني قبل أن تتحول النخلة لحطب

    [Header("VFX Settings")]
    public GameObject dustEffectPrefab; // أضف هذا السطر هنا

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = true;
    }

    void Update()
    {
        if (isPlayerTouching && !hasFallen)
        {
            if (AdvancedToolManager.currentToolName == requiredToolName)
            {
                StartFalling();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = false;
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

        // استدعاء وظيفة التحويل بعد وقت معين
        StartCoroutine(ConvertToWood());
    }

    IEnumerator ConvertToWood()
    {
        // انتظر الوقت المحدد (مثلاً 3 ثوانٍ حتى تستقر النخلة على الأرض)
        yield return new WaitForSeconds(delayBeforeTransform);

        // إنشاء قطع الحطب
        for (int i = 0; i < woodCount; i++)
        {
            // نضع الخشب في نفس مكان النخلة مع رفع بسيط لكل قطعة
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), i * 0.5f, Random.Range(-0.5f, 0.5f));
            Instantiate(woodLogPrefab, spawnPos, Random.rotation);
        }
        // إنشاء تأثير غبار في مكان النخلة عند اختفائها
        if (dustEffectPrefab != null)
        {
            Instantiate(dustEffectPrefab, transform.position, Quaternion.identity);
        }

        // حذف النخلة من المشهد
        Destroy(gameObject);
    }
   
}