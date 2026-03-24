using UnityEngine;
using UnityEngine.UI;

public class ToDoManagmentScript : MonoBehaviour
{
    public static ToDoManagmentScript Instance;

    [Header("Items To Pick Up")]
    public GameObject penToPickUp;
    public GameObject rulerToPickUp;
    public GameObject eraserToPickUp;

    [Header("UI Panel")]
    public GameObject todoPanel;

    [Header("Task UI (Images or Text)")]
    public Image penCheck;
    public Image rulerCheck;
    public Image eraserCheck;

    private bool penDone = false;
    private bool rulerDone = false;
    private bool eraserDone = false;

    private void Awake()
    {
        // ✅ Singleton like your CheckpointManager
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Optional: hide checks at start
        if (penCheck != null) penCheck.enabled = false;
        if (rulerCheck != null) rulerCheck.enabled = false;
        if (eraserCheck != null) eraserCheck.enabled = false;

        if (todoPanel != null)
            todoPanel.SetActive(true);
    }

    void Update()
    {
        CheckAllTasks();
    }

    // ✅ This is what CollectItem will call
    public void CollectItem(GameObject item)
    {
        if (item == penToPickUp && !penDone)
        {
            penDone = true;
            if (penCheck != null) penCheck.enabled = true;
        }
        else if (item == rulerToPickUp && !rulerDone)
        {
            rulerDone = true;
            if (rulerCheck != null) rulerCheck.enabled = true;
        }
        else if (item == eraserToPickUp && !eraserDone)
        {
            eraserDone = true;
            if (eraserCheck != null) eraserCheck.enabled = true;
        }
    }

    void CheckAllTasks()
    {
        if (penDone && rulerDone && eraserDone)
        {
            Debug.Log("All Tasks Completed!");

            // ✅ Example: hide panel
            if (todoPanel != null)
                todoPanel.SetActive(false);

            // 👉 You can trigger win / open door here
        }
    }
}