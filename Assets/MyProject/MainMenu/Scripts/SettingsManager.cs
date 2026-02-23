using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    private bool soundEnabled = true;
    private bool musicEnabled = true;
    private string language = "EN";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAllSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ==============================
    // SAVE METHODS
    // ==============================

    public void SaveSound(bool isEnabled)
    {
        soundEnabled = isEnabled;
        PlayerPrefs.SetInt("SoundEnabled", isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveMusic(bool isEnabled)
    {
        musicEnabled = isEnabled;
        PlayerPrefs.SetInt("MusicEnabled", isEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveLanguage(string lang)
    {
        language = lang;
        PlayerPrefs.SetString("Language", lang);
        PlayerPrefs.Save();
    }

    // ==============================
    // LOAD METHODS
    // ==============================

    private void LoadAllSettings()
    {
        soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        musicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        language = PlayerPrefs.GetString("Language", "EN");

        ApplySettings();
    }

    // ==============================
    // APPLY SETTINGS
    // ==============================

    private void ApplySettings()
    {
        // Sound (SFX) – usually affects AudioListener
        AudioListener.pause = !soundEnabled;

        // Music (if you have a music AudioSource in scene)
        if (musicSource != null)
            musicSource.mute = !musicEnabled;

        Debug.Log("Sound: " + soundEnabled);
        Debug.Log("Music: " + musicEnabled);
        Debug.Log("Language: " + language);
    }

    // Assign this in Inspector
    public AudioSource musicSource;

    // ==============================
    // GET METHODS
    // ==============================

    public bool IsSoundEnabled() => soundEnabled;
    public bool IsMusicEnabled() => musicEnabled;
    public string GetLanguage() => language;
}