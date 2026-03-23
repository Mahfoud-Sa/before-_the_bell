using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Start()
    {
        if (player != null)
            transform.position = new Vector3(player.position.x, player.position.y, -12);
    }

    private void LateUpdate()
    {
        if (player != null)
            transform.position = new Vector3(player.position.x, player.position.y, -12);
    }
}