using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    public static EndGameUI Instance;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private void Awake()
    {

        Instance = this;
    }

    private void Start()
    {
        Debug.Log("StartPanel should show now");

        // Show start panel
        startPanel.SetActive(true);
        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        // Pause the game
        Time.timeScale = 0f;
    }

    // ▶ Play Button
    public void StartGame()
    {
        startPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Change if needed
    }
}