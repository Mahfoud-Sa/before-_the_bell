using UnityEngine;
using System.Collections.Generic;

public class TreeScript : MonoBehaviour
{
    [Header("Seeds Settings")]
    public GameObject seedPrefab;     
    public int minSeeds = 1;          
    public int maxSeeds = 3;          

    [Header("Tree Size Settings")]
    public Vector3 treeScale = new Vector3(0.20f, 0.20f, 1f); // حجم الشجرة

    [Header("Coins Settings")]
    public GameObject coinPrefab;     
    public int minCoins = 1;          
    public int maxCoins = 4;          
    public float coinSpawnZ = -50f;   

    [Header("Audio")]
    public AudioClip chopSound;

    [Header("Seed Distribution")]
    public float seedRadius = 0.7f; // نصف القطر حول الشجرة

    private List<GameObject> currentSeeds = new List<GameObject>();
    private Vector3 originalScale;
    private bool isChopped = false;

    private void Start()
    {
        originalScale = transform.localScale;

        if (gameObject.name.Contains("(Clone)"))
        {
            transform.localScale = treeScale;
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && !isChopped)
        {
            string tool = AdvancedToolManager.currentToolName;
            playerMove player = FindObjectOfType<playerMove>();

            if (tool == "Shrim")
            {
                ChopTree(player);
            }
        }
    }

    void ChopTree(playerMove player)
    {
        isChopped = true;
 // 🔥 INFORM GAMEMANAGER HERE
    if (GameManager.Instance != null)
    {
        GameManager.Instance.DecreaseTreeCount();
    }
        if (chopSound != null)
        {
            AudioSource.PlayClipAtPoint(chopSound, Camera.main.transform.position);
        }

        DropLoot();

        if (player != null)
            player.StartActionAnim();

        // تصغير الشجرة بدل إخفائها
        transform.localScale *= 0.5f;
    }

    void DropLoot()
    {
        // ====== Drop Seeds ======
        if (seedPrefab != null)
        {
            currentSeeds.Clear(); 
            int seedsToDrop = Random.Range(minSeeds, maxSeeds + 1);

            for (int i = 0; i < seedsToDrop; i++)
            {
                Vector3 seedPos = transform.position;

                // توزيع البذور حول الشجرة في دائرة
                float angle = 360f / seedsToDrop * i;
                float rad = angle * Mathf.Deg2Rad;

                seedPos.x += Mathf.Cos(rad) * seedRadius;
                seedPos.y += Mathf.Sin(rad) * seedRadius;
                seedPos.z = 0f; // ثابت Z = 0

                GameObject spawnedSeed = Instantiate(seedPrefab, seedPos, Quaternion.identity);

                Seed seedScript = spawnedSeed.GetComponent<Seed>();
                if (seedScript != null)
                {
                    seedScript.originalTree = this.gameObject;
                    seedScript.index = i;
                    seedScript.totalSeeds = seedsToDrop;
                }

                currentSeeds.Add(spawnedSeed);
            }
        }

        // ====== Drop Coins ======
        if (coinPrefab != null)
        {
            int coinsToDrop = Random.Range(minCoins, maxCoins + 1);

            for (int i = 0; i < coinsToDrop; i++)
            {
                Vector3 coinPos = transform.position;

                coinPos.z = coinSpawnZ;
                coinPos.x += Random.Range(-1.5f, 1.5f);
                coinPos.y += Random.Range(0f, 1.0f);

                Instantiate(coinPrefab, coinPos, Quaternion.identity);
            }
        }
    }

    public void RestoreTreeAndDestroySeeds()
    {
        foreach (GameObject seed in currentSeeds)
        {
            if (seed != null)
                Destroy(seed);
        }

        currentSeeds.Clear();
        transform.localScale = originalScale;
        isChopped = false;
    }
}