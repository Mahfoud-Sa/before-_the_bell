using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject rockPrefab;
    public GameObject bigRockPrefab;
    public Transform spawnParent;
    public Transform playerTransform;
    public Rigidbody playerRigidbody;

    [Header("Spawn Settings")]
    public int maxActiveRocks = 8;
    public float spawnInterval = 0.2f;
    public float spawnHeight = 15f;
    public float horizontalSpread = 0.3f;

    [Header("Wave Settings")]
    public float waveDuration = 6f;
    public float wavePause = 3.5f;

    [Header("Prediction & Clamping")]
    public float predictionTime = 0.12f;
    public float maxDistanceFromPlayer = 1.0f;

    [Header("Lead Distance Progression")]
    public float startLead = 4.0f;
    public float midLead = 2.0f;
    public float endLead = 0.4f;
    public float timeToMid = 10f;
    public float timeToEnd = 20f;

    [Header("Initial Fall Speed Progression (regular rocks)")]
    public float startInitialFallSpeed = 8f;
    public float endInitialFallSpeed = 18f;

    [Header("Physics & Safety (regular rocks)")]
    public float gravityMultiplier = 1.0f;
    public bool enableContinuousCollision = true;

    [Header("Special Big Rock Settings")]
    public bool enableBigRock = true;
    public float specialSpawnDistance = 7f;
    public float specialInitialFallSpeed = 40f;
    public float specialGravityMultiplier = 1.2f;
    public float specialSpawnInterval = 2f;
    public int maxActiveBigRocks = 2;

    [Header("Landing lifetimes (seconds)")]
    [Tooltip("Time (s) a small rock stays on ground before being destroyed")]
    public float smallLandingLifetime = 5f;
    [Tooltip("Time (s) a big rock stays on ground before being destroyed")]
    public float bigLandingLifetime = 9f;

    // داخلي
    private bool isSpawning = false;
    private List<GameObject> activeRocks = new List<GameObject>();
    private List<GameObject> activeBigRocks = new List<GameObject>();
    private Coroutine waveCoroutine;
    private float timeInsideZone = 0f;
    private float persistentBigTimer = 0f; // يحفظ عبر الموجات

    public void StartSpawning(Transform player)
    {
        if (player == null) return;
        playerTransform = player;
        if (playerRigidbody == null) playerRigidbody = player.GetComponent<Rigidbody>();

        if (isSpawning) return;
        isSpawning = true;
        timeInsideZone = 0f;
        persistentBigTimer = 0f;
        waveCoroutine = StartCoroutine(WaveLoop());
        Debug.Log("[RockSpawner] StartSpawning");
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;
        isSpawning = false;
        if (waveCoroutine != null) StopCoroutine(waveCoroutine);
        Debug.Log("[RockSpawner] StopSpawning");
    }

    private IEnumerator WaveLoop()
    {
        while (isSpawning)
        {
            float waveElapsed = 0f;

            while (isSpawning && waveElapsed < waveDuration)
            {
                // توليد الأحجار الاعتيادية
                timeInsideZone += spawnInterval;
                activeRocks.RemoveAll(r => r == null);
                activeBigRocks.RemoveAll(r => r == null);

                if (activeRocks.Count < maxActiveRocks)
                    SpawnRegularOne();

                // big rock timer (يحفظ عبر الموجات)
                if (enableBigRock && bigRockPrefab != null)
                {
                    persistentBigTimer += spawnInterval;
                    if (persistentBigTimer >= specialSpawnInterval)
                    {
                        persistentBigTimer = 0f;
                        TrySpawnBigRock();
                    }
                }

                waveElapsed += spawnInterval;
                yield return new WaitForSeconds(spawnInterval);
            }

            // وقفة موجة
            float pauseT = 0f;
            while (isSpawning && pauseT < wavePause)
            {
                pauseT += Time.deltaTime;
                yield return null;
            }
        }
    }

    #region Regular rock spawn
    private float ComputeLeadDistance()
    {
        if (timeInsideZone >= timeToEnd) return endLead;
        if (timeInsideZone >= timeToMid) return midLead;
        float t = Mathf.Clamp01(timeInsideZone / Mathf.Max(0.0001f, timeToMid));
        return Mathf.Lerp(startLead, midLead, t);
    }

    private float ComputeInitialFallSpeed()
    {
        if (timeInsideZone >= timeToEnd) return endInitialFallSpeed;
        float t = Mathf.Clamp01(timeInsideZone / Mathf.Max(0.0001f, timeToEnd));
        return Mathf.Lerp(startInitialFallSpeed, endInitialFallSpeed, t);
    }

    private void SpawnRegularOne()
    {
        if (rockPrefab == null || playerTransform == null) return;

        float predictedX = playerTransform.position.x;
        if (playerRigidbody != null) predictedX += playerRigidbody.linearVelocity.x * predictionTime;

        float currentLead = ComputeLeadDistance();
        bool playerIsMoving = (playerRigidbody != null) && (Mathf.Abs(playerRigidbody.linearVelocity.x) > 0.05f);

        float baseTargetX = playerIsMoving ? predictedX : (playerTransform.position.x + currentLead * Mathf.Sign((playerRigidbody != null) ? playerRigidbody.linearVelocity.x : 1f));
        float targetX = baseTargetX + UnityEngine.Random.Range(-horizontalSpread, horizontalSpread);

        float dx = Mathf.Clamp(targetX - playerTransform.position.x, -maxDistanceFromPlayer, maxDistanceFromPlayer);
        float spawnX = playerTransform.position.x + dx;
        Vector3 spawnPos = new Vector3(spawnX, spawnHeight, playerTransform.position.z);

        GameObject rock = Instantiate(rockPrefab, spawnPos, Quaternion.identity, spawnParent);

        Rigidbody rb = rock.GetComponent<Rigidbody>();
        if (rb != null && enableContinuousCollision) rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        float currentInit = ComputeInitialFallSpeed();
        FallingRock fr = rock.GetComponent<FallingRock>();
        if (fr != null) fr.Init(this, currentInit, gravityMultiplier, smallLandingLifetime);
        fr.isBigRock = false;
        activeRocks.Add(rock);
        Debug.Log($"[RockSpawner] Regular spawned at X={spawnX:F2} initFall={currentInit:F2}");
    }
    #endregion

    #region Big rock spawn
    private void TrySpawnBigRock()
    {
        activeBigRocks.RemoveAll(r => r == null);
        if (activeBigRocks.Count >= maxActiveBigRocks) return;
        if (bigRockPrefab == null || playerTransform == null) return;

        float dir = 1f;
        if (playerRigidbody != null && Mathf.Abs(playerRigidbody.linearVelocity.x) > 0.05f)
            dir = Mathf.Sign(playerRigidbody.linearVelocity.x);

        float spawnX = playerTransform.position.x + dir * specialSpawnDistance + UnityEngine.Random.Range(-horizontalSpread, horizontalSpread);
        Vector3 spawnPos = new Vector3(spawnX, spawnHeight, playerTransform.position.z);

        GameObject bigRock = Instantiate(bigRockPrefab, spawnPos, Quaternion.identity, spawnParent);
        
        Rigidbody rb = bigRock.GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (enableContinuousCollision) rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        FallingRock fr = bigRock.GetComponent<FallingRock>();
        if (fr != null) fr.Init(this, specialInitialFallSpeed, specialGravityMultiplier, bigLandingLifetime);
        fr.isBigRock = true;

        activeBigRocks.Add(bigRock);
        activeRocks.Add(bigRock);
        Debug.Log($"[RockSpawner] BIG spawned at X={spawnX:F2}, dist={specialSpawnDistance}, initFall={specialInitialFallSpeed}");
    }

    // دالة عامة للاختبار اليدوي إن أردت
    public void TrySpawnBigRock_Public()
    {
        TrySpawnBigRock();
    }
    #endregion

    // يُستدعى من FallingRock عند تدميره
    public void NotifyRockDestroyed(GameObject rock)
    {
        if (activeRocks.Contains(rock)) activeRocks.Remove(rock);
        if (activeBigRocks.Contains(rock)) activeBigRocks.Remove(rock);
    }

    // حذف كل الأحجار وإعادة الضبط (عند Respawn)
    public void ClearAllRocks()
    {
        foreach (var r in activeRocks) if (r != null) Destroy(r);
        activeRocks.Clear();
        activeBigRocks.Clear();
        timeInsideZone = 0f;
        persistentBigTimer = 0f;
    }
}
