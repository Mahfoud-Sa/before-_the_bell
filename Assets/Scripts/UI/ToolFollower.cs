using UnityEngine;

public class ToolFollower : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        transform.position = mainCam.ScreenToWorldPoint(mousePos);
    }
}