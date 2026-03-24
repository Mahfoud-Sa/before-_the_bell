using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("🟢 TestScript Awake called — script is running!");
    }

    private void Start()
    {
        Debug.Log("🟢 TestScript Start called — script is running!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("🟢 Space pressed — Update works!");
        }
    }
}
