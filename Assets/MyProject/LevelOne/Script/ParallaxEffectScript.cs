using UnityEngine;

public class ParallaxEffectScript : MonoBehaviour
{
    Material mat;
    float distance;
    [Range(0f, 5f)]
    public float speed=0.2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mat=GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        distance += Time.deltaTime * speed;
        mat.SetTextureOffset("_MainTex", new Vector2(distance, 0));
    }
}
