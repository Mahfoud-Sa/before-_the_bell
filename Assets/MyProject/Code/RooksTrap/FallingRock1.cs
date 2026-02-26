using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class FallingRock1 : MonoBehaviour
{
    [Header("Collision")]
    public LayerMask groundLayer;
    public GameObject dustPrefab;

    // lifetime after landing (set by spawner per-rock)
    [HideInInspector] public float landingLifetime = 3f;

    private Rigidbody rb;
    private RockSpawner spawner;
    private bool hasLanded = false;
    private bool triggered = false;

    // fall control (set by spawner)
    private float gravityMultiplier = 1f;
    private float initialDownVelocity = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Init called by spawner.
    /// landingLifetime: time in seconds to destroy this rock after it lands.
    /// </summary>
    public void Init(RockSpawner parentSpawner, float initialDownVel = 0f, float gravityMult = 1f, float landingLifetimeSec = 3f)
    {
        spawner = parentSpawner;
        initialDownVelocity = Mathf.Abs(initialDownVel);
        gravityMultiplier = Mathf.Max(0.01f, gravityMult);
        this.landingLifetime = Mathf.Max(0.01f, landingLifetimeSec);

        rb.isKinematic = false;
        rb.useGravity = true;

        if (initialDownVelocity > 0f)
        {
            Vector3 v = rb.linearVelocity;
            v.y = -initialDownVelocity;
            rb.linearVelocity = v;
        }
    }

    void FixedUpdate()
    {
        if (gravityMultiplier != 1f && rb != null)
        {
            Vector3 extraAccel = (gravityMultiplier - 1f) * Physics.gravity;
            rb.AddForce(extraAccel * rb.mass, ForceMode.Force);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (triggered) return;

        // إصابة اللاعب أثناء السقوط (قبل الهبوط)
        if (!hasLanded && collision.gameObject.CompareTag("Player"))
        {
            triggered = true;
            CheckpointManager.Instance.Respawn();
            return;
        }

        // عند الاصطدام بالأرض: نجعل الحجر ثابتاً ونشغّل الغبار ونبدأ مؤقت الحذف
        if (!hasLanded && ((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            hasLanded = true;

            // غبار
            if (dustPrefab != null && collision.contacts.Length > 0)
            {
                ContactPoint cp = collision.contacts[0];
                GameObject dust = Instantiate(dustPrefab, new Vector3(cp.point.x,cp.point.y - 0.5f,cp.point.z), Quaternion.identity);
                ParticleSystem ps = dust.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
                Destroy(dust, 6f);
            }

            // نجعل الجسم ثابتاً في مكانه: هذا يمنع أي حركة لاحقة ويتيح للاعب القفز فوقه
            // استخدم isKinematic = true ليكون ثابتاً تماماً.
            // البديل: rb.constraints = RigidbodyConstraints.FreezeAll;
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                // نضبط isKinematic true ليصبح الحجر ثابتاً في مكانه
                rb.isKinematic = true;
            }

            // نبدأ تدمير الحجر بعد landingLifetime (قيمة يمكن ضبطها من spawner)
            StartCoroutine(DestroyAfterLanding(landingLifetime));
        }
    }

    private IEnumerator DestroyAfterLanding(float time)
    {
        yield return new WaitForSeconds(time);

        // أبلغ المسبّح واعمل Destroy
        if (spawner != null)
            spawner.NotifyRockDestroyed(gameObject);

        Destroy(gameObject);
    }

    // تدمير فوري إن احتجت
    public void ForceDestroy()
    {
        if (spawner != null) spawner.NotifyRockDestroyed(gameObject);
        Destroy(gameObject);
    }
}
