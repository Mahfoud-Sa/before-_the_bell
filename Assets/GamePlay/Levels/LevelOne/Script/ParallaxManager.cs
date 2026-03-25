using UnityEngine;
using System;

[Serializable]
public struct Background
{
    [HideInInspector] public string name;

    [Range(0, 1)]
    [Tooltip("0 = Far (Sky), 1 = Near (Ground)")]
    public float intensity;

    [Header("Layer Depth")]
    [Tooltip("This controls the actual Z position for sprite sorting")]
    public float zPosition;

    public Transform sprite;
}

public class ParallaxManager : MonoBehaviour
{
    public Transform gameCamera;

    [SerializeField] private Background[] backgrounds;

    [Header("Settings")]
    [Tooltip("Vertical parallax strength")]
    [SerializeField] private float heightMultiplier = 0.2f;

    public static ParallaxManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (gameCamera == null && Camera.main != null)
            gameCamera = Camera.main.transform;
    }

    void Update()
    {
        // The Parent (Parallax System) should be following the camera via your Follow script
        Vector3 parentPos = transform.position;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].sprite == null) continue;

            // 🧠 CALCULATING SLIP
            // Higher intensity = more slip (stays closer to world origin)
            // Lower intensity = less slip (stays closer to camera center)
            float offsetX = parentPos.x * backgrounds[i].intensity;
            float offsetY = parentPos.y * backgrounds[i].intensity * heightMultiplier;

            // 🎯 APPLYING POSITION
            // We use zPosition for the actual Z axis to handle sorting/depth
            backgrounds[i].sprite.localPosition = new Vector3(
                -offsetX, 
                -offsetY, 
                backgrounds[i].zPosition 
            );
        }
    }

    // --- EDITOR METHODS (Fixed for your Editor Script) ---

    public void ResetBackgrounds()
    {
        backgrounds = new Background[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            backgrounds[i].sprite = transform.GetChild(i);
            backgrounds[i].name = backgrounds[i].sprite.name;
            backgrounds[i].intensity = (float)i / (transform.childCount > 1 ? transform.childCount - 1 : 1);
            backgrounds[i].zPosition = i * 2f; // Incremental Z for sorting
        }
    }

    public void ResetIntensities()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].intensity = (float)i / (backgrounds.Length > 1 ? backgrounds.Length - 1 : 1);
        }
    }

    public void ResetZScales()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Assign Z positions in increments so they don't Z-fight (overlap)
            backgrounds[i].zPosition = i * 5f; 
        }
    }

    public void AutoSetZFromIntensity()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Map Intensity (0 to 1) to Z Position (50 to 0)
            // Far things (low intensity) get high Z (away from camera)
            backgrounds[i].zPosition = Mathf.Lerp(50f, 0f, backgrounds[i].intensity);
        }
    }
}