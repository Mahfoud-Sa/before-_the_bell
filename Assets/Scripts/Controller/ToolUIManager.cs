// ToolUIManager.cs
using UnityEngine;
using UnityEngine.UI;


public class ToolUIManager : MonoBehaviour
{
    public GameObject toolsPanel;
    public Button mainToolButton;
    public Image[] toolImages;
    public Button[] toollImageButtons;
    public GameObject[] toolObjects;
    private bool isPanelOpen = false;

    private void Start()
    {
        mainToolButton.onClick.AddListener(ToogleToolsPanel);
        if (toolImages.Length != toollImageButtons.Length)
        {
            Debug.LogError("the number of picture not equel the number of burrons");
            return;
        }
        for (int i = 0; i< toollImageButtons.Length; i++)
        {
            int index = i;
            toollImageButtons[i].onClick.AddListener(() => SelectTool(index));
        }
        HideAllTools();
        toolsPanel.SetActive(false);

    }
    void ToogleToolsPanel()
    {
        isPanelOpen = !isPanelOpen;
        toolsPanel.SetActive(isPanelOpen);
    }
    void SelectTool(int toolIndex)
    {
        HideAllTools();
        if(toolIndex >= 0 && toolIndex < toolObjects.Length)
        {
            toolObjects[toolIndex].SetActive(true);
        }
        toolsPanel.SetActive(false);
        isPanelOpen=false;

    }
    void HideAllTools()
    {
        foreach (GameObject tool in toolObjects)
        {
            tool.SetActive(false);
        }
    }
}