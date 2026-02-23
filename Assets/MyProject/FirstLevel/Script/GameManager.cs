using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GoatRandomMovement goat;
    public Transform holdPosition;

    public void OnButtonPressed()
    {
        if (goat != null && holdPosition != null)
        {
            goat.Hold(holdPosition);
        }
    }
}