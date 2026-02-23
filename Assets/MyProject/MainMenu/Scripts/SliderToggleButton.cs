using UnityEngine;
using UnityEngine.UI;


public class SliderToggleButton : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private Image buttonImage;
    private bool isOn = true;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        UpdateVisual();
    }

    public void Toggle()
    {
        isOn = !isOn;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        buttonImage.sprite = isOn ? activeSprite : inactiveSprite;
    }
}

