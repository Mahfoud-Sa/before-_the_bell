using UnityEngine;
using UnityEngine.UI;

public class AdvancedToolManager : MonoBehaviour
{
    public Canvas toolsPanelCanvas;
    public GraphicRaycaster toolsPanelRaycaster;
    
    public Button openButton;
    public Button closeButton;
    
    public Button[] toolButtons;
    public Sprite[] toolSprites;
    
    public SpriteRenderer activeToolRenderer;
    public static string currentToolName = "";

    private int currentSelectedToolIndex = -1;

    private void Start()
    {
        openButton.onClick.AddListener(OpenPanel);
        closeButton.onClick.AddListener(ClosePanel);

        for (int i = 0; i < toolButtons.Length; i++)
        {
            int index = i;
            toolButtons[i].onClick.AddListener(() => SelectTool(index));
        }

        ClosePanel();
        DeselectAllTools();
    }

    private void OpenPanel()
    {
        if (toolsPanelCanvas != null) toolsPanelCanvas.enabled = true;
        if (toolsPanelRaycaster != null) toolsPanelRaycaster.enabled = true;
    }

    private void ClosePanel()
    {
        if (toolsPanelCanvas != null) toolsPanelCanvas.enabled = false;
        if (toolsPanelRaycaster != null) toolsPanelRaycaster.enabled = false;
    }

    private void SelectTool(int index)
    {
        if (currentSelectedToolIndex == index)
        {
            DeselectAllTools();
        }
        else
        {
            currentSelectedToolIndex = index;
            
            if (index >= 0 && index < toolSprites.Length)
            {
                activeToolRenderer.sprite = toolSprites[index];
                currentToolName = toolSprites[index].name;
            }
            
            for (int i = 0; i < toolButtons.Length; i++)
            {
                toolButtons[i].interactable = (i == index);
            }
        }
    }

    private void DeselectAllTools()
    {
        currentSelectedToolIndex = -1;
        
        if (activeToolRenderer != null)
        {
            activeToolRenderer.sprite = null;
        }
        
        currentToolName = "";
        
        for (int i = 0; i < toolButtons.Length; i++)
        {
            if (toolButtons[i] != null)
            {
                toolButtons[i].interactable = true;
            }
        }
    }
}