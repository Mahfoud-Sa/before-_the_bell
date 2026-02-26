using UnityEngine;
using System.Collections.Generic;

public class TreeScript : MonoBehaviour
{
    public GameObject seedPrefab;     
    public int minSeeds = 1;          
    public int maxSeeds = 3;          

    public GameObject coinPrefab;     
    public int minCoins = 1;          
    public int maxCoins = 4;          
    public float coinSpawnZ = -50f;   

    public AudioClip chopSound;

    private List<GameObject> currentSeeds = new List<GameObject>();

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            string tool = AdvancedToolManager.currentToolName;
            playerMove player = FindObjectOfType<playerMove>();

            if (tool == "Shrim")
            {
                if (chopSound != null)
                {
                    AudioSource.PlayClipAtPoint(chopSound, Camera.main.transform.position);
                }

                DropLoot();

                if (player != null) player.StartActionAnim();

                gameObject.SetActive(false);
            }
        }
    }

    void DropLoot()
    {
        if (seedPrefab != null)
        {
            currentSeeds.Clear(); 
            int seedsToDrop = Random.Range(minSeeds, maxSeeds + 1);
            for (int i = 0; i < seedsToDrop; i++)
            {
                Vector3 seedPos = transform.position;
                seedPos.z = seedPrefab.transform.position.z; 

                GameObject spawnedSeed = Instantiate(seedPrefab, seedPos, Quaternion.identity);
                Seed seedScript = spawnedSeed.GetComponent<Seed>();
                if (seedScript != null)
                {
                    seedScript.originalTree = this.gameObject;
                }
                currentSeeds.Add(spawnedSeed);
            }
        }

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
            if (seed != null) Destroy(seed);
        }
        currentSeeds.Clear(); 
        gameObject.SetActive(true);
    }
}