using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{
    [Header("Animation Settings")]
    public float dropDuration = 0.5f;
    public float spreadDistance = 1.0f;
    public float jumpHeight = 1.5f;
    
    [Header("Fly To UI Settings")]
    public float flySpeed = 15f;
    public int coinValue = 1;

    private bool isCollected = false;

    void Start()
    {
        // تناثر العملة بمجرد ظهورها
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
        if (Input.GetMouseButtonDown(0) && !isCollected)
        {
            isCollected = true;
            StartCoroutine(FlyToUI());
        }
    }

    IEnumerator FlyToUI()
    {
        if (CoinManager.Instance == null || CoinManager.Instance.coinText == null)
        {
            CollectCoin();
            yield break;
        }

        Camera mainCam = Camera.main;
        
        while (true)
        {
            Vector3 uiScreenPos = CoinManager.Instance.coinText.transform.position;
            
            uiScreenPos.z = Mathf.Abs(mainCam.transform.position.z - transform.position.z);
            Vector3 targetWorldPos = mainCam.ScreenToWorldPoint(uiScreenPos);

            transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, flySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWorldPos) < 0.5f)
            {
                CollectCoin();
                yield break;
            }
            
            yield return null;
        }
    }

    void CollectCoin()
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoins(coinValue);
        }
        Destroy(gameObject);
    }
}