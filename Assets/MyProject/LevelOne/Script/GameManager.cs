using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject settingsPanel;
    private bool isSettingsOpen = false;
    public float gameTime = 60f;
    private float currentTime;
    private bool gameEnded = false;

    void Start()
    {
        currentTime = gameTime;
        Time.timeScale = 1f;
    }
// Open / Close Settings
public void ToggleSettings()
{
    if (isSettingsOpen)
        CloseSettings();
    else
        OpenSettings();
}

public void OpenSettings()
{
    settingsPanel.SetActive(true);
    Time.timeScale = 0f;
    isSettingsOpen = true;
}

public void CloseSettings()
{
    settingsPanel.SetActive(false);
    Time.timeScale = 1f;
    isSettingsOpen = false;
}
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
{
    ToggleSettings();
}
        if (gameEnded)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        EndGameUI.Instance.ShowGameOver();
    }

// Go To Main Menu
public void GoToMainMenu()
{
    Time.timeScale = 1f;
    UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene"); // change name if needed
}
    public void WinGame()
    {
        if (gameEnded) return;

        gameEnded = true;
        EndGameUI.Instance.ShowWin();
    }
}