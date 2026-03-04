using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Settings")]
    public GameObject settingsPanel;
    private bool isSettingsOpen = false;

    [Header("Timer")]
    public float gameTime = 60f;
    private float currentTime;
    private bool gameEnded = false;

    [Header("Stars System")]
    public int maxStars = 3;
    public int currentStars = 2; // Player starts with 2 stars

    // Optional flags for star logic
    public bool usedEffect = false;
    public bool boughtFromStore = false;

    // ---------------- TREE SYSTEM ----------------

    [Header("Tree System")]
    [Tooltip("Set how many trees must be chopped to open the wall")]
    public int remainingTrees = 3;   // 👈 Set this manually in Inspector

    public GameObject treeWall;      // 👈 Assign the wall here


    // ---------------- UNITY METHODS ----------------

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

    // ---------------- TREE LOGIC ----------------

    public void DecreaseTreeCount()
    {
        if (gameEnded) return;

        remainingTrees--;

        Debug.Log("Remaining Trees: " + remainingTrees);

        if (remainingTrees <= 0)
        {
            OpenWall();
        }
    }

    private void OpenWall()
    {
        if (treeWall != null)
        {
            treeWall.SetActive(false);
        }

        Debug.Log("Wall Opened!");
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
        Time.timeScale = 0f;

        if (EndGameUI.Instance != null)
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