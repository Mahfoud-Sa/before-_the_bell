using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [Header("UI References")]
    public GameObject loadingPanel;         // Panel containing your LoadingImage
    [SerializeField] private float minDisplayTime = 2f;  // Minimum time animation stays visible

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (loadingPanel != null)
                loadingPanel.SetActive(false); // hide panel at start
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load scene by name
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    // Load scene by index (optional)
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneRoutine(sceneIndex));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true); // Show panel + animation

        float timer = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        // Keep animation visible for at least minDisplayTime
        while (!operation.isDone || timer < minDisplayTime)
        {
            timer += Time.deltaTime;

            // Activate scene when fully loaded AND minimum time passed
            if (operation.progress >= 0.9f && timer >= minDisplayTime)
                operation.allowSceneActivation = true;

            yield return null;
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false); // Hide panel after load
    }

    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        float timer = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone || timer < minDisplayTime)
        {
            timer += Time.deltaTime;

            if (operation.progress >= 0.9f && timer >= minDisplayTime)
                operation.allowSceneActivation = true;

            yield return null;
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}