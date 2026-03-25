using UnityEngine;
using UnityEngine.UI;

public class ToggleMusicButton : MonoBehaviour
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
        SoundManager.Instance.ToggleMusic();
        UpdateVisual();
    }

    void UpdateVisual()
    {
        bool isMuted = SoundManager.Instance.IsMusicMuted();
        buttonImage.sprite = isMuted ? inactiveSprite : activeSprite;
    }
}