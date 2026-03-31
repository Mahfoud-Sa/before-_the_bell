using UnityEngine;
using System.Collections;

public class PalmBridge : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasFallen = false;
    private bool isPlayerTouching = false;
    private bool isBridge = false;

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

    private void OnTriggerEnter(Collider other)
    {
        if (hasFallen && !isBridge)
        {
            if (other.GetComponent<MudAreaZoneScript>() != null || other.GetComponent<MudAreaScript>() != null)
            {
                BecomeBridge(other);
            }
        }
    }

    void BecomeBridge(Collider mudCollider)
    {
        isBridge = true;
        
        // Stop the wood transformation
        StopAllCoroutines();

        // Freeze physics
        rb.isKinematic = true;

        // Disable the mud trigger so the player can walk freely over the bridge
        mudCollider.enabled = false;

        Debug.Log("🌴 Tree became a bridge over the mud!");
    }

    void StartFalling()
    {
        hasFallen = true;

        // تعطيل الفيزياء العادية لأننا سنجعلها تسقط برمجياً لضمان الدقة
        rb.isKinematic = true;
        rb.useGravity = false;

        // تحديد جهة السقوط
        float torqueDir = -1f; // سالب = لليمين، موجب = لليسار

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (player.transform.position.x > transform.position.x)
                torqueDir = 1f; // تسقط لليسار بعيداً عن اللاعب
            else
                torqueDir = -1f; // تسقط لليمين
        }

        Debug.Log("Tree falling...");

        StartCoroutine(FallAnimation(torqueDir));
    }

    IEnumerator FallAnimation(float dir)
    {
        float duration = 1.0f;
        float elapsed = 0f;
        
        // نقطة الارتكاز (Pivot) هي أسفل النخلة لتسقط كأنها مقطوعة فعلياً
        Collider coll = GetComponent<Collider>();
        Vector3 pivot = transform.position;
        if (coll != null)
        {
            pivot = transform.position - new Vector3(0, coll.bounds.extents.y, 0);
        }
        
        // الإزاحة المطلوبة لكي تبعد عن الجذع المقطوع وتسقط على مستوى الأرض بشكل مريح
        // (dir سالب يعني لليمين، موجب म्हणजे لليسار) فا نستخدم -dir لندفعها بنفس الاتجاه
        Vector3 detachOffset = new Vector3(-dir * 1.5f, -0.6f, 0f);

        while (elapsed < duration)
        {
            float t = Time.deltaTime / duration;
            float step = 90f * t;
            
            transform.RotateAround(pivot, Vector3.forward * dir, step);

            // الانزلاق تدريجياً لتبعد عن الجذع وتنزل على الأرض
            transform.position += detachOffset * t;
            pivot += detachOffset * t; // تحديث الارتكاز حتى يستمر الدوران بسلاسة
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        // فحص ما إذا كانت الشجرة بعد سقوطها تلامس منطقة وحل
        bool hitMud = false;
        if (coll != null)
        {
            Collider[] hits = Physics.OverlapBox(coll.bounds.center, coll.bounds.extents + Vector3.one * 0.1f);
            foreach (var hit in hits)
            {
                if (hit.GetComponent<MudAreaZoneScript>() != null || hit.GetComponent<MudAreaScript>() != null)
                {
                    BecomeBridge(hit);
                    hitMud = true;
                    break;
                }
            }
        }

        // إذا لم تلامس الوحلة، تتحول إلى خشب
        if (!hitMud)
        {
            StartCoroutine(ConvertToWood());
        }
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