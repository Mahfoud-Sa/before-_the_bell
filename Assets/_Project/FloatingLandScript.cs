using UnityEngine;

public class FloatingLandScript : MonoBehaviour
{
   public GameObject platformPrefab;

    public int numberOfPlatforms = 20;
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    public float minY = 2f;
    public float maxY = 10f;

    void Start()
    {
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY),
                Random.Range(minZ, maxZ)
            );

            Instantiate(platformPrefab, randomPosition, Quaternion.identity);
        }
    }
}
