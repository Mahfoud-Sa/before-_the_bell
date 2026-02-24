using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;      // صوت الخلفية
    public AudioSource sfxSource;        // مؤثرات عامة
    public AudioSource loopSource;       // للأصوات المتكررة مثل الجري
    public AudioSource playerSounds;      
    public AudioSource rockSounds;      

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("Player Sounds")]
    public AudioClip jumpSound;
    public AudioClip runLoopSound;
    public AudioClip playerHitSound;

    [Header("Rock Sounds")]
    public AudioClip smallRockImpact;
    public AudioClip bigRockImpact;

    [Header("Coin")]
    public AudioClip coinPickupSound;

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
        PlayMusic();
    }

    // ---------- MUSIC ----------
    public void PlayMusic()
    {
        if (backgroundMusic == null) return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // ---------- ONE SHOT SFX ----------
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlayPlayerSounds(AudioClip clip)
    {
        if (clip == null) return;
        playerSounds.PlayOneShot(clip);
    }
    public void PlayRockSounds(AudioClip clip)
    {
        if (clip == null) return;
        rockSounds.PlayOneShot(clip);
    }
    // ---------- RUN LOOP ----------
    public void StartRunSound()
    {
        if (runLoopSound == null) return;

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

    // ---------- Public API ----------
    public void PlayJump() => PlayPlayerSounds(jumpSound);
    public void PlayPlayerHit() => PlayPlayerSounds(playerHitSound);
    public void PlaySmallRock() => PlayRockSounds(smallRockImpact);
    public void PlayBigRock() => PlaySFX(bigRockImpact);
    public void PlayCoin() => PlaySFX(coinPickupSound);
}