using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;      // صوت الخلفية
    public AudioSource sfxSource;        // مؤثرات عامة
    public AudioSource loopSource;       // للأصوات المتكررة مثل الجري
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
        playerSource.PlayOneShot(clip);
    }
    public void PlayRiverSounds(AudioClip clip)
    {
        if (clip == null) return;
        riverSource.PlayOneShot(clip);
    }
    public void PlayRockSounds(AudioClip clip)
    {
        if (clip == null) return;
        rockSource.PlayOneShot(clip);
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
    public void StopRiverSource()
    {
        if (riverSource.isPlaying)
            riverSource.Stop();
    }

    // ---------- Public API ----------
    // ---------- Player ----------
    public void PlayJump() => PlayPlayerSounds(jumpSound);
    public void PlayPlayerHit() => PlayPlayerSounds(playerHitSound);
    // ---------- Rocks ----------
    public void PlayComingRockSound() => PlayRockSounds(comingStone);
    public void PlaySmallRock() => PlayRockSounds(smallRockImpact);
    public void PlayBigRock() => PlaySFX(bigRockImpact);
    public void PlayAfterFallingStones() => PlaySFX(afterFallingStone);
    // ---------- Coin----------
    public void PlayCoin() => PlaySFX(coinPickupSound);
    // ---------- Gots----------
    public void PlayGotsSound() => PlaySFX(gotsSound);
    // ---------- RiverSound----------
    public void PlayRiverSound() => PlayRiverSounds(riverSound);
    public void StopRiverSound() => StopRiverSource();


    // ---------- WinSound----------
    public void PlayWinSound() => PlaySFX(winSound);
    // ---------- LoseSound----------
    public void PlayLoseSound() => PlaySFX(loseSound);
    // ---------- BellSound----------
    public void PlayBellSound() => PlaySFX(bellSound);

}
