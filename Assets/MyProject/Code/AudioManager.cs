using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //[Header("----------- Audio Source -------")]

    public AudioSource musicSource;
    public AudioSource SFXSource;

    //[Header("----------- AudioClip Source -------")]

    public AudioClip background;
    public AudioClip death;
    public AudioClip getPoint;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {

        SFXSource.PlayOneShot(clip);

    }

    public void StopMusic()
    {

        musicSource.Stop();

    }
}
