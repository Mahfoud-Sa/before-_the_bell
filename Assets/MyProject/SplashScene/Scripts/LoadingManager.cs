using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [Header("UI Reference")]
    public GameObject loadingPanel;   // Drag your LoadingPanel here

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep it between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Optional: prevent scene from activating immediately
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            yield return null;
        }

        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }
}