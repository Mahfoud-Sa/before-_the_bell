using UnityEngine;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    [Header("Splash Settings")]
    [SerializeField] private float splashDuration = 5f;      // Duration of splash screen
    [SerializeField] private string nextSceneName = "MainMenu"; // Scene to load after splash

    void Start()
    {
        StartCoroutine(PlaySplashAndLoad());
    }

    private IEnumerator PlaySplashAndLoad()
    {
        // 1️⃣ Wait for splash screen duration
        yield return new WaitForSeconds(splashDuration);

        // 2️⃣ Load next scene using LoadingManager
        if (LoadingManager.Instance != null)
        {
            LoadingManager.Instance.LoadScene(nextSceneName);
        }
        else
        {
            // Fallback if LoadingManager is missing
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}