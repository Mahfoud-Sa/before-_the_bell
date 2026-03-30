using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [Header("UI Elements")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    //public RectTransform handPointer;

    [Header("Checklist")]
    public GameObject checklistPanel;
    public GameObject[] checklistTasks;

    [Header("Timings")]
    public float welcomeReadTime = 2.0f;
    public float listReadTime = 1.5f;
    public float transitionDelay = 0.1f;

    [Header("Tasks Settings")]
    public int requiredTrees = 5;
    private bool treeMissionDone = false;
    private bool waterMissionDone = false;
public void StartTutorialFromTrigger()
{
    if (currentStep != TutorialStep.Welcome) return;

    if (tutorialPanel != null)
        tutorialPanel.SetActive(true);

    StartCoroutine(WelcomeSequence());
}
    public enum TutorialStep { Welcome, OpenPanelForShrim, SelectShrim, CutTree, SelectGardel, FullWater }
    public TutorialStep currentStep = TutorialStep.Welcome;

    private bool isTransitioning = false;
    private Coroutine typeWriterCoroutine;

    private void Awake() { Instance = this; }

    private void Start()
    {
       // if (handPointer != null) handPointer.gameObject.SetActive(false);
        UpdateTutorialState();
    }

    private void Update()
    {
    }

    public void UpdateTutorialState()
    {
        //if (handPointer != null) handPointer.gameObject.SetActive(false);

        switch (currentStep)
        {
            case TutorialStep.Welcome:
                StartCoroutine(WelcomeSequence());
                break;

            case TutorialStep.OpenPanelForShrim:
                ShowMessage("Click hand to open the tool bar.");
                //if (handPointer != null) handPointer.gameObject.SetActive(true);
                break;

            case TutorialStep.SelectShrim:
                ShowMessage("Select the Shrim tool.");
                break;

            case TutorialStep.CutTree:
                ShowMessage("Cut 5 trees.");
                break;

            case TutorialStep.SelectGardel:
                ShowMessage("Select the Empty Gardel.");
                break;

            case TutorialStep.FullWater:
                ShowMessage("Go to the well and fill water.");
                break;
        }
    }

    private void ShowMessage(string msg)
    {
        if (tutorialPanel != null && !tutorialPanel.activeSelf)
            tutorialPanel.SetActive(true);

        if (typeWriterCoroutine != null)
            StopCoroutine(typeWriterCoroutine);

        typeWriterCoroutine = StartCoroutine(TypewriterEffect(msg));
    }

    private IEnumerator TypewriterEffect(string message)
    {
        tutorialText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            tutorialText.text += letter;
            yield return new WaitForSeconds(0.015f);
        }
    }

    public void CompleteAction(TutorialStep actionStep)
    {
        // 👇 مهمة الماء
        if (actionStep == TutorialStep.FullWater)
        {
            waterMissionDone = true;

            if (checklistTasks.Length > 1)
                checklistTasks[1].SetActive(false);

            CheckAllMissionsComplete();
            return;
        }

        if (currentStep == actionStep && !isTransitioning)
        {
            StartCoroutine(TransitionToNextStep(actionStep + 1));
        }
    }

    public void CheckTreeMission(int count)
    {
        if (!treeMissionDone && count >= requiredTrees)
        {
            treeMissionDone = true;

            if (checklistTasks.Length > 0)
                checklistTasks[0].SetActive(false);

            // 👇 الانتقال لمرحلة الجردل
            currentStep = TutorialStep.SelectGardel;
            UpdateTutorialState();

            CheckAllMissionsComplete();
        }
    }

    void CheckAllMissionsComplete()
    {
        if (treeMissionDone && waterMissionDone)
        {
            ShowMessage("Tasks completed successfully!");

            if (checklistPanel != null)
                checklistPanel.SetActive(false);

            Invoke("HideTutorial", 3f);
        }
    }

    private IEnumerator TransitionToNextStep(TutorialStep nextStep)
    {
        isTransitioning = true;
        yield return new WaitForSeconds(transitionDelay);
        currentStep = nextStep;
        UpdateTutorialState();
        isTransitioning = false;
    }

    private IEnumerator WelcomeSequence()
    {
        ShowMessage("Before going to school, complete these tasks");
        yield return new WaitForSeconds(welcomeReadTime);

        if (checklistPanel != null)
            checklistPanel.SetActive(true);

        yield return new WaitForSeconds(listReadTime);

        currentStep = TutorialStep.OpenPanelForShrim;
        UpdateTutorialState();
    }

    void HideTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
    }
}