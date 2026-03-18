using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource loopSource;
    public AudioSource playerSource;
    public AudioSource rockSource;
    public AudioSource riverSource;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("River")]
    public AudioClip riverSound;

    [Header("Gots")]
    public AudioClip gotsSound;

    [Header("Win")]
    public AudioClip winSound;

    [Header("Lose")]
    public AudioClip loseSound;

    [Header("Bell Sound")]
    public AudioClip bellSound;

    [Header("Player Sounds")]
    public AudioClip jumpSound;
    public AudioClip runLoopSound;
    public AudioClip playerHitSound;

    [Header("Rock Sounds")]
    public AudioClip smallRockImpact;
    public AudioClip bigRockImpact;
    public AudioClip afterFallingStone;
    public AudioClip comingStone;

    [Header("Coin")]
    public AudioClip coinPickupSound;

    [Header("Mute Settings")]
    private bool isMusicMuted = false;
    private bool isSFXMuted = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadSettings();
        ApplyMuteSettings();
        PlayMusic();
    }

    // ---------- LOAD ----------
    void LoadSettings()
    {
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        isSFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
    }

    void ApplyMuteSettings()
    {
        musicSource.mute = isMusicMuted;

        sfxSource.mute = isSFXMuted;
        loopSource.mute = isSFXMuted;
        playerSource.mute = isSFXMuted;
        rockSource.mute = isSFXMuted;
        riverSource.mute = isSFXMuted;
    }

    // ---------- GETTERS ----------
    public bool IsMusicMuted() => isMusicMuted;
    public bool IsSFXMuted() => isSFXMuted;

    // ---------- TOGGLES ----------
    public void ToggleMusic()
    {
        isMusicMuted = !isMusicMuted;
        musicSource.mute = isMusicMuted;

        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
    }

    public void ToggleSFX()
    {
        isSFXMuted = !isSFXMuted;

        sfxSource.mute = isSFXMuted;
        loopSource.mute = isSFXMuted;
        playerSource.mute = isSFXMuted;
        rockSource.mute = isSFXMuted;
        riverSource.mute = isSFXMuted;

        PlayerPrefs.SetInt("SFXMuted", isSFXMuted ? 1 : 0);
    }

    // ---------- MUSIC ----------
    public void PlayMusic()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // ---------- SFX ----------
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || isSFXMuted) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayPlayerSounds(AudioClip clip)
    {
        if (clip == null || isSFXMuted) return;
        playerSource.PlayOneShot(clip);
    }

    public void PlayRiverSounds(AudioClip clip)
    {
        if (clip == null || isSFXMuted) return;
        riverSource.PlayOneShot(clip);
    }

    public void PlayRockSounds(AudioClip clip)
    {
        if (clip == null || isSFXMuted) return;
        rockSource.PlayOneShot(clip);
    }

    // ---------- LOOP ----------
    public void StartRunSound()
    {
        if (runLoopSound == null || isSFXMuted) return;

        if (!loopSource.isPlaying)
        {
            loopSource.clip = runLoopSound;
            loopSource.loop = true;
            loopSource.Play();
        }
    }

    public void StopRunSound()
    {
        if (loopSource.isPlaying)
            loopSource.Stop();
    }

    public void StopRiverSource()
    {
        if (riverSource.isPlaying)
            riverSource.Stop();
    }

    // ---------- PUBLIC API ----------
    public void PlayJump() => PlayPlayerSounds(jumpSound);
    public void PlayPlayerHit() => PlayPlayerSounds(playerHitSound);

    public void PlayComingRockSound() => PlayRockSounds(comingStone);
    public void PlaySmallRock() => PlayRockSounds(smallRockImpact);
    public void PlayBigRock() => PlaySFX(bigRockImpact);
    public void PlayAfterFallingStones() => PlayRiverSounds(afterFallingStone);

    public void PlayCoin() => PlaySFX(coinPickupSound);
    public void PlayGotsSound() => PlaySFX(gotsSound);

    public void PlayRiverSound() => PlayRiverSounds(riverSound);
    public void StopRiverSound() => StopRiverSource();

    public void PlayWinSound() => PlaySFX(winSound);
    public void PlayLoseSound() => PlaySFX(loseSound);
    public void PlayBellSound() => PlaySFX(bellSound);
}