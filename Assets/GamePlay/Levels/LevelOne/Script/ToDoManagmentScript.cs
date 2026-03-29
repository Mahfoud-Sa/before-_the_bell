using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToDoManagementScript : MonoBehaviour
{
    public static ToDoManagementScript Instance;

    [System.Serializable]
    public class QuestItem
    {
        public string itemName;
        public GameObject worldObject; 
        public Image uiCheckMark;      
        [HideInInspector] public bool isCollected = false;
    }

    [Header("Quest List")]
    public List<QuestItem> questItems = new List<QuestItem>();

    [Header("UI Panel Settings")]
    public GameObject todoPanel;
    public Button toggleButton; // Optional: Assign your "Show Items" button here

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Setup initial UI state
        foreach (var item in questItems)
        {
            if (item.uiCheckMark != null) 
                item.uiCheckMark.enabled = item.isCollected;
        }

        // Start with the panel hidden
        if (todoPanel != null)
            todoPanel.SetActive(false);

        // If you assigned the button in Inspector, we hook it up automatically
        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleToDoMenu);
    }

    // --- NEW TOGGLE METHOD ---
    public void ToggleToDoMenu()
    {
        if (todoPanel != null)
        {
            bool isActive = !todoPanel.activeSelf;
            todoPanel.SetActive(isActive);

            // Optional: Pause game time or play a sound when opening
            if (isActive) 
            {
                Debug.Log("Opening To-Do List...");
                RefreshUI(); // Ensure checkmarks match current progress
            }
        }
        else
        {
            Debug.LogWarning("todoPanel is not assigned in the Inspector!");
        }
    }

  public void CollectItem(GameObject collectedObject)
{
    foreach (var item in questItems)
    {
        if (item.worldObject == null) continue;

        // Compare by instance OR name (safer)
        if ((item.worldObject == collectedObject || 
             item.worldObject.name == collectedObject.name) 
             && !item.isCollected)
        {
            item.isCollected = true;

            // Activate checkmark properly
            if (item.uiCheckMark != null)
                item.uiCheckMark.gameObject.SetActive(true);
                item.uiCheckMark.color = Color.red;
                item.uiCheckMark.rectTransform.sizeDelta = new Vector2(100, 100);
                item.uiCheckMark.transform.SetAsLastSibling();
                item.uiCheckMark.enabled = true;
            Debug.Log($"{item.itemName} collected and UI updated!");

           // CheckAllTasks();
            return;
        }
    }

    Debug.LogWarning("Collected object not found in quest list!");
}

    // Updates all checkmarks based on the isCollected bool
    void RefreshUI()
    {
        foreach (var item in questItems)
        {
            if (item.uiCheckMark != null)
                item.uiCheckMark.enabled = item.isCollected;
        }
    }

    // void CheckAllTasks()
    // {
    //     if (questItems.TrueForAll(x => x.isCollected))
    //     {
    //         Debug.Log("All Tasks Completed!");
    //         // You might want to keep the panel open for a moment to show the final check
    //     }
    // }
}