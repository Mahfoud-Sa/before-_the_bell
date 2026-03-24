using UnityEngine;

public class BackgroundAutoFit : MonoBehaviour
{
     void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Camera cam = Camera.main;

        float worldScreenHeight = cam.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

        Vector2 spriteSize = sr.sprite.bounds.size;

        transform.localScale = new Vector3(
            worldScreenWidth / spriteSize.x,
            worldScreenHeight / spriteSize.y,
            1
        );
    }
}
