using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int coins = 0;

    [Header("Settings")]
    public GameObject settingsPanel;
    private bool isSettingsOpen = false;

    [Header("Timer")]
    public float gameTime = 60f;
    private float currentTime;
    private bool gameEnded = false;

    [Header("Stars System")]
    public int maxStars = 3;
    public int currentStars = 2;

    public bool usedEffect = false;
    public bool boughtFromStore = false;

    [Header("Tree System")]
    public int remainingTrees = 3;
    public GameObject treeWall;

    [Header("Goat System")]
    public int currentGoats = 0;
    public int maxGoats = 10;

    // ---------------- UNITY METHODS ----------------

    private void Awake()
    {
        // Safe Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // VERY IMPORTANT: prevents Android freeze
      //  Time.timeScale = 1f;
    }

    void Start()
    {
        currentTime = gameTime;
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

    // ---------------- COINS ----------------

    public void AddCoins(int amount)
    {
        coins += amount;

        Debug.Log("💰 Coins: " + coins);

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateCoinsText(coins);
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
            treeWall.SetActive(false);

        Debug.Log("Wall Opened!");
    }

    // ---------------- GOAT LOGIC ----------------

    public void AddGoat()
    {
        if (currentGoats < maxGoats)
        {
            currentGoats++;
            Debug.Log("Goat Added. Total Goats: " + currentGoats);
        }
    }

    public void RemoveGoat()
    {
        if (currentGoats > 0)
        {
            currentGoats--;
            Debug.Log("Goat Removed. Remaining Goats: " + currentGoats);
        }
    }

    public int GetGoatCount()
    {
        return currentGoats;
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
            Debug.Log("Star Added: " + currentStars);
        }
    }

    public void LoseStar()
    {
        if (currentStars > 0)
        {
            currentStars--;
            Debug.Log("Star Lost: " + currentStars);
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
        if (gameEnded) return;

        gameEnded = true;

        if (usedEffect)
            LoseStar();

        if (boughtFromStore)
            AddStar();

        Time.timeScale = 0f;

        Debug.Log("Player Won!");
    }

    // ---------------- NAVIGATION ----------------

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    // ---------------- TIMER ACCESS ----------------

    public float GetCurrentTime()
    {
        return currentTime;
    }
}