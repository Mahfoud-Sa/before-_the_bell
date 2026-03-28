using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] private Transform cam; // Main Camera // Main Camera
    Vector3 camStartPos;
    float distance; // distance between camera start position and current position

    GameObject[] backgrounds;
    Material[] mat;
    float[] backSpeed;

    float farthestBack;

    [Range(0.01f, 0.05f)]
    public float parallaxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Find the main camera
        cam = Camera.main?.transform;
        if (cam == null)
        {
            Debug.LogError("No Main Camera found! Make sure your camera has the 'MainCamera' tag.");
            return; // stop further setup
        }

        camStartPos = cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;

            Renderer rend = backgrounds[i].GetComponent<Renderer>();
            if (rend != null)
            {
                mat[i] = rend.material;
            }
            else
            {
                Debug.LogWarning("Child '" + backgrounds[i].name + "' does not have a Renderer! Parallax effect will not apply.");
                mat[i] = null; // prevent crash later
            }
        }

        BackSpeedCalculate(backCount);
    }

    void BackSpeedCalculate(int backCount)
    {
        if (cam == null) return; // safety check

        // Find the farthest background
        for (int i = 0; i < backCount; i++)
        {
            float zDistance = backgrounds[i].transform.position.z - cam.position.z;
            if (zDistance > farthestBack)
            {
                farthestBack = zDistance;
            }
        }

        // Set the speed of each background
        for (int i = 0; i < backCount; i++)
        {
            float zDistance = backgrounds[i].transform.position.z - cam.position.z;
            if (farthestBack != 0)
                backSpeed[i] = 1 - (zDistance / farthestBack);
            else
                backSpeed[i] = 0; // prevent division by zero
        }
    }

    private void LateUpdate()
    {
        if (cam == null) return; // safety check

        distance = cam.position.x - camStartPos.x;
        transform.position = new Vector3(cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (mat[i] != null) // only apply if material exists
            {
                float speed = backSpeed[i] * parallaxSpeed;
                mat[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
            }
        }
    }
}