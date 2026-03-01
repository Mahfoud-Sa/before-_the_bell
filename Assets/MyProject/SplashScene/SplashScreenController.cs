using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    [SerializeField] private float splashDuration = 5f;
    [SerializeField] private string nextSceneName = "MainMenu";

    void Start()
    {
        StartCoroutine(LoadAfterDelay());
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(splashDuration);
        SceneManager.LoadScene(nextSceneName);
    }
}