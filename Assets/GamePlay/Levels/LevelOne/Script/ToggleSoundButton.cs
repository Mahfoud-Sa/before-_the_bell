using UnityEngine;
using UnityEngine.UI;

public class ToggleSoundButton : MonoBehaviour
{
    public Sprite activeSprite;
    public Sprite inactiveSprite;

    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        UpdateVisual();
    }

    public void Toggle()
    {
        SoundManager.Instance.ToggleSFX();
        UpdateVisual();
    }

    void UpdateVisual()
    {
        bool isMuted = SoundManager.Instance.IsSFXMuted();
        buttonImage.sprite = isMuted ? inactiveSprite : activeSprite;
    }
}