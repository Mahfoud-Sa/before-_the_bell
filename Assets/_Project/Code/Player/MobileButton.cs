using UnityEngine;
using UnityEngine.EventSystems; // Required for handling button presses

public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private PlayerMovement player; // Drag your Player object here
    [SerializeField] private bool isRightButton;    // Check this box if this is the RIGHT button
    [SerializeField] private bool isLeftButton;     // Check this box if this is the LEFT button

    // When you touch the button
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRightButton) player.PressRight(true);
        if (isLeftButton) player.PressLeft(true);
    }

    // When you let go (or slide off) the button
    public void OnPointerUp(PointerEventData eventData)
    {
        if (isRightButton) player.PressRight(false);
        if (isLeftButton) player.PressLeft(false);
    }
}