// FallingRock.cs (جزء متعلق بالهبوط والإنشاء)
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class FallingRock : MonoBehaviour
{
    public LayerMask groundLayer;
    public GameObject dustPrefab;
    public bool isBigRock = false;
    [Header("Coins on landing")]
    public GameObject coinPrefab;          // assign coin prefab in inspector (can be null)
    public int coinsOnLanding = 1;         // عدد العملات التي تُولّد عند الهبوط
    public float coinSpawnRadius = 0.6f;   // تشتت عملات عند الظهور
    public float coinLifetime = 12f;       // وقت بقاء العملة إذا لم تُجمع

    [HideInInspector] public float landingLifetime = 3f;

    private Rigidbody rb;
    private RockSpawner spawner;
    private bool hasLanded = false;
    private bool triggered = false;
    private float gravityMultiplier = 1f;
    private float initialDownVelocity = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

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

        // إصابة اللاعب أثناء السقوط
        if (!hasLanded && collision.gameObject.CompareTag("Player"))
        {
            triggered = true;
            SoundManager.Instance?.PlayPlayerHit();
            CheckpointManager.Instance.Respawn();

            return;
        }

        // هبوط على الأرض
        if (!hasLanded && ((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            hasLanded = true;

            // موقع الاصطدام
            ContactPoint cp = collision.contacts.Length > 0 ? collision.contacts[0] : default(ContactPoint);
            Vector3 contactPoint = (collision.contacts.Length > 0) ? cp.point : transform.position;
            // صوت اصطدام الحجر
            if (isBigRock)
                SoundManager.Instance?.PlayBigRock();
            else
                SoundManager.Instance?.PlaySmallRock();
           
            // — 1) غبار
            if (dustPrefab != null)
            {
                GameObject dust = Instantiate(dustPrefab, contactPoint, Quaternion.identity);
                ParticleSystem ps = dust.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play();
                Destroy(dust, 6f);
            }

            // — 2) عملات عند الهبوط
            if (coinPrefab != null && coinsOnLanding > 0)
            {
                SpawnCoinsAt(contactPoint, coinsOnLanding);
            }

            // — 3) تثبيت الجسم في مكانه
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
            SoundManager.Instance.PlayAfterFallingStones();
            // — 4) ابدأ تدمير الحجر بعد landingLifetime
            StartCoroutine(DestroyAfterLanding(landingLifetime));
        }
    }

    private void SpawnCoinsAt(Vector3 center, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = Random.insideUnitSphere * coinSpawnRadius;
            offset.y = 0f; // اجعل التشتت أفقيًا بحيث تبقى على الأرض
            Vector3 pos = center + offset;

            GameObject c = Instantiate(coinPrefab, pos, Quaternion.identity);
            // تأكد أن العملة لديها Collider isTrigger وتستمع للاعب
            // اعطِها وقت انتهاء العرض
            Destroy(c, coinLifetime);
        }
    }

    private IEnumerator DestroyAfterLanding(float time)
    {
        yield return new WaitForSeconds(time);

        if (spawner != null) spawner.NotifyRockDestroyed(gameObject);
        Destroy(gameObject);
    }
}
