using UnityEngine;
using UnityEngine.UI; // Only needed if you use UI Image

public class MusicSliderScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
  public Sprite newSprite; // Drag the sprite you want to switch to
    private SpriteRenderer spriteRenderer; // For 2D object
    private Image uiImage; // For UI object

    private bool isChanged = false;

    void Start()
    {
        // Check if it's a UI Image
        uiImage = GetComponent<Image>();
        if (uiImage == null)
        {
            // Otherwise, assume it's a 2D SpriteRenderer
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void OnMouseDown()
    {
        // Called when the object is clicked (works for SpriteRenderer)
        ChangeSprite();
    }

    public void ChangeSprite()
    {
       // if (isChanged) return; // optional: only change once

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
        else if (uiImage != null)
        {
            uiImage.sprite = newSprite;
        }

        //isChanged = true;
    }
}
