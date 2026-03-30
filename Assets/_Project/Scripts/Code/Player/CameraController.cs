using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform player;

    [Header("Camera Settings")]
    [SerializeField] private float zOffset = -12f;

    [Header("Basement Lock Position")]
    [SerializeField] private Vector3 basementPosition = new Vector3(476.78f, -23.51f, -33.9f);

    private bool isLocked = false;

    private void Start()
    {
        FollowPlayer();
    }

    private void LateUpdate()
    {
        if (isLocked)
        {
            transform.position = basementPosition;
        }
        else
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        if (player == null) return;

        transform.position = new Vector3(
            player.position.x,
            player.position.y,
            zOffset
        );
    }

    public void LockCamera()
    {
        isLocked = true;
    }

    public void UnlockCamera()
    {
        isLocked = false;
    }
}