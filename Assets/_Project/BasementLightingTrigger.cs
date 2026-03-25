using UnityEngine;
using System.Collections;

public class GlobalLightingTrigger : MonoBehaviour
{
    [Header("Main Light (Sun)")]
    public Light mainLight;

    [Header("Basement Lights")]
    public Transform basementLightsParent;
    private Light[] basementLights;

    [Header("Player")]
    public GameObject player;

    [Header("Animation Settings")]
    public float lightFadeDuration = 1f; // seconds
    public float ambientFadeDuration = 1f;

    // ===== ORIGINAL SETTINGS =====
    private Color originalAmbient;
    private float originalAmbientIntensity;
    private float originalReflectionIntensity;
    private Material originalSkybox;
    private bool originalFog;
    private Color originalFogColor;
    private float originalFogDensity;

    // ===== DARK SETTINGS =====
    [Header("Dark Mode Settings")]
    public Color darkAmbient = Color.black;
    public float darkAmbientIntensity = 0f;
    public float darkReflectionIntensity = 0f;
    public bool enableFog = true;
    public Color fogColor = Color.black;
    public float fogDensity = 0.03f;

    private void Start()
    {
        basementLights = basementLightsParent.GetComponentsInChildren<Light>(true);

        originalAmbient = RenderSettings.ambientLight;
        originalAmbientIntensity = RenderSettings.ambientIntensity;
        originalReflectionIntensity = RenderSettings.reflectionIntensity;
        originalSkybox = RenderSettings.skybox;

        originalFog = RenderSettings.fog;
        originalFogColor = RenderSettings.fogColor;
        originalFogDensity = RenderSettings.fogDensity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(EnterDarkModeAnimated());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ExitDarkModeAnimated());
        }
    }

    // =========================
    // ENTER DARK MODE ANIMATED
    // =========================
    IEnumerator EnterDarkModeAnimated()
    {
        // 1. Disable player lights gradually
        DisablePlayerLights();

        // 2. Fade main light OFF
        if (mainLight != null)
            yield return StartCoroutine(FadeLightIntensity(mainLight, mainLight.intensity, 0f, lightFadeDuration));

        // 3. Enable basement lights gradually
        foreach (Light l in basementLights)
        {
            l.enabled = true;
            l.intensity = 0f;
            StartCoroutine(FadeLightIntensity(l, 0f, 1f, lightFadeDuration));
        }

        // 4. Fade ambient light
        yield return StartCoroutine(FadeAmbient(originalAmbientIntensity, darkAmbientIntensity, ambientFadeDuration));

        // 5. Enable fog immediately
        RenderSettings.fog = enableFog;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = fogDensity;

        // 6. Remove skybox
        RenderSettings.skybox = null;

        // 7. Update lighting
        DynamicGI.UpdateEnvironment();
    }

    // =========================
    // EXIT DARK MODE ANIMATED
    // =========================
    IEnumerator ExitDarkModeAnimated()
    {
        // 1. Fade basement lights OFF
        foreach (Light l in basementLights)
        {
            StartCoroutine(FadeLightIntensity(l, l.intensity, 0f, lightFadeDuration));
        }

        // 2. Fade main light ON
        if (mainLight != null)
            yield return StartCoroutine(FadeLightIntensity(mainLight, mainLight.intensity, 1f, lightFadeDuration));

        // 3. Fade ambient back to original
        yield return StartCoroutine(FadeAmbient(RenderSettings.ambientIntensity, originalAmbientIntensity, ambientFadeDuration));
        RenderSettings.ambientLight = originalAmbient;
        RenderSettings.reflectionIntensity = originalReflectionIntensity;
        RenderSettings.skybox = originalSkybox;

        // 4. Restore fog
        RenderSettings.fog = originalFog;
        RenderSettings.fogColor = originalFogColor;
        RenderSettings.fogDensity = originalFogDensity;

        // 5. Enable player lights
        EnablePlayerLights();

        // 6. Update lighting
        DynamicGI.UpdateEnvironment();
    }

    // =========================
    // FADE FUNCTIONS
    // =========================
    IEnumerator FadeLightIntensity(Light light, float start, float end, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            light.intensity = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }
        light.intensity = end;
    }

    IEnumerator FadeAmbient(float startIntensity, float endIntensity, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            RenderSettings.ambientIntensity = Mathf.Lerp(startIntensity, endIntensity, time / duration);
            yield return null;
        }
        RenderSettings.ambientIntensity = endIntensity;
    }

    // =========================
    // PLAYER LIGHT CONTROL
    // =========================
    void DisablePlayerLights()
    {
        if (player == null) return;
        Light[] lights = player.GetComponentsInChildren<Light>(true);
        foreach (Light l in lights)
        {
            l.enabled = false;
        }
    }

    void EnablePlayerLights()
    {
        if (player == null) return;
        Light[] lights = player.GetComponentsInChildren<Light>(true);
        foreach (Light l in lights)
        {
            l.enabled = true;
        }
    }
}