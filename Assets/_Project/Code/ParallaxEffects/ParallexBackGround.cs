using UnityEngine;

public class ParallexBackGround : MonoBehaviour
{
    public float startPosition, length;
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    [SerializeField] private Vector2 parallexEffectMultiplier;

    private void Start() 
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate() 
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallexEffectMultiplier.x, deltaMovement.y * parallexEffectMultiplier.y, 0);
        lastCameraPosition = cameraTransform.position;

        // if (cameraTransform.position.x - transform.position.x > textureUnitSizeX)
        // {
        //     float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
        //     transform.position = new Vector3(cameraTransform.position.x + offsetPositionX, transform.position.y);
        // }
        
    }
    void FixedUpdate()
    {
        float temp = cameraTransform.position.x * (1 - parallexEffectMultiplier.x);
        float dist = cameraTransform.position.x * parallexEffectMultiplier.x;

        transform.position = new Vector3(startPosition + dist, transform.position.y, transform.position.z);

        if (temp > startPosition + length)
        {
            startPosition += length;
        }
        else if (temp < startPosition - length)
        {
            startPosition -= length;
        }

    }





}

