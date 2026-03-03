using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject settingsPanel;
    private bool isSettingsOpen = false;

    [Header("Timer")]
    public float gameTime = 60f;
    private float currentTime;
    private bool gameEnded = false;

    [Header("Stars System")]
    public int maxStars = 3;
    public int currentStars = 2; // Player starts with 2 star

    // Optional flags for star logic
    public bool usedEffect = false;
    public bool boughtFromStore = false;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentTime = gameTime;
        Time.timeScale = 1f;
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

    // ---------------- SETTINGS ----------------

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

    // ---------------- STARS ----------------

    public void AddStar()
    {
        if (currentStars < maxStars)
        {
            currentStars++;
            Debug.Log("Star Added. Current Stars: " + currentStars);
        }
    }

    public void LoseStar()
    {
        if (currentStars > 0)
        {
            currentStars--;
            Debug.Log("Star Lost. Current Stars: " + currentStars);
        }
    }

    // ---------------- GAME STATES ----------------

    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        EndGameUI.Instance.ShowGameOver();
    }

   public void WinGame()
    {
        Debug.Log("WinGame called in GameManager");
        if (gameEnded) return;

        gameEnded = true;

        if (usedEffect)
            LoseStar();

        if (boughtFromStore)
            AddStar();

        Time.timeScale = 0f;
    }

    // ---------------- NAVIGATION ----------------

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}