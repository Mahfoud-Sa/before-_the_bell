using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Coin Settings")]
    public int value = 5;

    [Header("Rotation Settings")]
    public float rotationSpeed = 200f;

    [Header("Sprites")]
    public Sprite frontSprite;
    public Sprite backSprite;

    [Header("Scale Settings")]
    public Vector3 baseScale = new Vector3(0.3f, 0.3f, 0.3f);

    private SpriteRenderer spriteRenderer;
    private float rotationY;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set initial size
        transform.localScale = baseScale;
    }

    private void Update()
    {
        rotationY += rotationSpeed * Time.deltaTime;
        if (rotationY > 360f) rotationY -= 360f;

        float rad = rotationY * Mathf.Deg2Rad;

        // Smooth flip scale
        float scaleX = Mathf.Cos(rad);

        // Apply scale while keeping your base size
        transform.localScale = new Vector3(
            baseScale.x * scaleX,
            baseScale.y,
            baseScale.z
        );

        // Swap sprite at edge
        if (Mathf.Abs(scaleX) < 0.05f)
        {
            if (rotationY <= 180f)
                spriteRenderer.sprite = backSprite;
            else
                spriteRenderer.sprite = frontSprite;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayCoin();

            if (CoinManager.Instance != null)
                CoinManager.Instance.AddCoins(value);

            Destroy(gameObject, 0.05f);
        }
    }
}