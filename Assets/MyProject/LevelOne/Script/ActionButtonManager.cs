using UnityEngine;
using UnityEngine.UI;

public class ActionButtonManager : MonoBehaviour
{
    public static ActionButtonManager Instance;

    public GameObject actionButton;
    public Image actionIcon;

    private System.Action currentAction;

    void Awake()
    {
        Instance = this;
        HideAction();
    }

    public void ShowAction(Sprite icon, System.Action action)
    {
        actionButton.SetActive(true);
        actionIcon.sprite = icon;
        currentAction = action;
    }

    public void HideAction()
    {
        actionButton.SetActive(false);
        currentAction = null;
    }

    public void OnActionPressed()
    {
        if (currentAction != null)
        {
            currentAction.Invoke();
        }
    }
}