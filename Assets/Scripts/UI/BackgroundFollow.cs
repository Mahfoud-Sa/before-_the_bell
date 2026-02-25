using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform player;
    [Range(0, 1)] public float followSpeed = 1f;

    private float startPosX;
    private float offset;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;
        }

        if (player != null)
        {
            startPosX = transform.position.x;
            offset = transform.position.x - player.position.x;
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            float targetX = (player.position.x + offset) * followSpeed;
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }
    }
}