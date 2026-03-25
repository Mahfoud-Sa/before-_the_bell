using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TranferPointTriggerScript : MonoBehaviour
{
    [Header("Target")]
    public Transform targetPoint;

    [Header("Fade UI")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Sound")]
    public AudioClip teleportSound;
    private AudioSource audioSource;

    private bool isTeleporting = false;

    private void Awake()
    {
        // Add AudioSource if not present
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            StartCoroutine(FadeTeleportWithSound(other.transform));
        }
    }

    IEnumerator FadeTeleportWithSound(Transform player)
    {
        isTeleporting = true;

        // 1. Fade OUT (to black)
        yield return StartCoroutine(Fade(0f, 1f));

        // 2. Play teleport sound
        if (teleportSound != null)
        {
            audioSource.PlayOneShot(teleportSound);
        }

        // 3. Teleport player
        player.position = targetPoint.position;
        player.rotation = targetPoint.rotation;

        // Small delay for smooth effect
        yield return new WaitForSeconds(0.2f);

        // 4. Fade IN (back to normal)
        yield return StartCoroutine(Fade(1f, 0f));

        isTeleporting = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        // Ensure final alpha
        color.a = endAlpha;
        fadeImage.color = color;
    }
}