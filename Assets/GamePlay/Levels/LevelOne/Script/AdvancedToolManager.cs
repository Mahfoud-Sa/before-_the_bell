using UnityEngine;
using UnityEngine.UI;

public class AdvancedToolManager : MonoBehaviour
{
    public static AdvancedToolManager Instance;

    public Sprite fullGardelSprite;
    public Canvas toolsPanelCanvas;
    public GraphicRaycaster toolsPanelRaycaster;

    public Button openButton;
    public Button closeButton;

    public Button[] toolButtons;
    public Sprite[] toolSprites;

    public SpriteRenderer activeToolRenderer;
    public static string currentToolName = "";

    public bool isGardelFull = false;

    private int currentSelectedToolIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

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

        if (TutorialManager.Instance != null)
            TutorialManager.Instance.CompleteAction(TutorialManager.TutorialStep.OpenPanelForShrim);
    }

    private void ClosePanel()
    {
        if (toolsPanelCanvas != null) toolsPanelCanvas.enabled = false;
        if (toolsPanelRaycaster != null) toolsPanelRaycaster.enabled = false;
    }

    private void SelectTool(int index)
    {
        // 👇 إذا ضغط نفس الأداة → إلغاء التحديد
        if (currentSelectedToolIndex == index)
        {
            DeselectAllTools();
            return;
        }

        currentSelectedToolIndex = index;

        if (index >= 0 && index < toolSprites.Length)
        {
            // 👇 لو الجردل ممتلئ
            if (toolSprites[index].name == "EmptyGardel" && isGardelFull)
            {
                activeToolRenderer.sprite = fullGardelSprite;
                currentToolName = "FullGardel";
            }
            else
            {
                activeToolRenderer.sprite = toolSprites[index];
                currentToolName = toolSprites[index].name;
            }

            // تعليمات التوتوريال
            if (TutorialManager.Instance != null)
            {
                if (currentToolName == "Shrim")
                    TutorialManager.Instance.CompleteAction(TutorialManager.TutorialStep.SelectShrim);

                else if (currentToolName == "EmptyGardel")
                    TutorialManager.Instance.CompleteAction(TutorialManager.TutorialStep.SelectGardel);
            }
        }

        // 👇 جعل باقي الأدوات باهتة
        UpdateToolsVisual();
    }
    void UpdateToolsVisual()
    {
        for (int i = 0; i < toolButtons.Length; i++)
        {
            Image img = toolButtons[i].GetComponent<Image>();

            if (img != null)
            {
                if (i == currentSelectedToolIndex)
                {
                    img.color = Color.white; // الأداة المختارة طبيعية
                }
                else
                {
                    img.color = new Color(1f, 1f, 1f, 0.4f); // باهت
                }
            }
        }
    }

    public void FillGardel()
    {
        isGardelFull = true;
        currentToolName = "FullGardel";

        // 👇 تغيير الشكل فقط في يد اللاعب
        if (currentSelectedToolIndex >= 0 && activeToolRenderer != null)
        {
            activeToolRenderer.sprite = fullGardelSprite;
        }

        Debug.Log("الجردل امتلأ وتغير شكله فقط في يد اللاعب");
    }

    public void ResetGardelToEmpty()
    {
        isGardelFull = false;
        currentToolName = "EmptyGardel";
    }

    public void DeselectAllTools()
    {
        currentSelectedToolIndex = -1;

        if (activeToolRenderer != null)
            activeToolRenderer.sprite = null;

        currentToolName = "";

        // 👇 رجوع كل الأدوات لطبيعتها
        for (int i = 0; i < toolButtons.Length; i++)
        {
            Image img = toolButtons[i].GetComponent<Image>();
            if (img != null)
                img.color = Color.white;
        }
    }
}

// using UnityEngine;
// using UnityEngine.UI;

// public class AdvancedToolManager : MonoBehaviour
// {
//     public Canvas toolsPanelCanvas;
//     public GraphicRaycaster toolsPanelRaycaster;
    
//     public Button openButton;
//     public Button closeButton;
    
//     public Button[] toolButtons;
//     public Sprite[] toolSprites;
    
//     public SpriteRenderer activeToolRenderer;
//     public static string currentToolName = "";

//     private int currentSelectedToolIndex = -1;

//     private void Start()
//     {
//         openButton.onClick.AddListener(OpenPanel);
//         closeButton.onClick.AddListener(ClosePanel);

//         for (int i = 0; i < toolButtons.Length; i++)
//         {
//             int index = i;
//             toolButtons[i].onClick.AddListener(() => SelectTool(index));
//         }

//         ClosePanel();
//         DeselectAllTools();
//     }

//     private void OpenPanel()
//     {
//         if (toolsPanelCanvas != null) toolsPanelCanvas.enabled = true;
//         if (toolsPanelRaycaster != null) toolsPanelRaycaster.enabled = true;
//     }

//     private void ClosePanel()
//     {
//         if (toolsPanelCanvas != null) toolsPanelCanvas.enabled = false;
//         if (toolsPanelRaycaster != null) toolsPanelRaycaster.enabled = false;
//     }

//     private void SelectTool(int index)
//     {
//         if (currentSelectedToolIndex == index)
//         {
//             DeselectAllTools();
//         }
//         else
//         {
//             currentSelectedToolIndex = index;
            
//             if (index >= 0 && index < toolSprites.Length)
//             {
//                 activeToolRenderer.sprite = toolSprites[index];
//                 currentToolName = toolSprites[index].name;
//             }
            
//             for (int i = 0; i < toolButtons.Length; i++)
//             {
//                 toolButtons[i].interactable = (i == index);
//             }
//         }
//     }

//     private void DeselectAllTools()
//     {
//         currentSelectedToolIndex = -1;
        
//         if (activeToolRenderer != null)
//         {
//             activeToolRenderer.sprite = null;
//         }
        
//         currentToolName = "";
        
//         for (int i = 0; i < toolButtons.Length; i++)
//         {
//             if (toolButtons[i] != null)
//             {
//                 toolButtons[i].interactable = true;
//             }
//         }
//     }
// }