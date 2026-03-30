using UnityEngine;

public class AxeButtonScript : MonoBehaviour
{
    
    public void OnAxePressed()
{
    Debug.Log("Axe Button Pressed");

    if (PalmBridge.currentPalm != null)
    {
        Debug.Log("Palm Found: " + PalmBridge.currentPalm.name);
        PalmBridge.currentPalm.OnAxeButtonPressed();
    }
    else
    {
        Debug.Log("No Palm Found");
    }
}
}
