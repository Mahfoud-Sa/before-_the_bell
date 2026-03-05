using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    public GameObject[] panels; 
    public int transitionIndex = 4;

    // Drag your button here in the Inspector
    public GameObject endButton;

    private int currentIndex = 0;

    void Start()
    {
        // Make sure button is hidden at start
        if (endButton != null)
            endButton.SetActive(true);
            endButton.GetComponent<Button>().interactable = false; 

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNext();
        }
    }

    void ShowNext()
    {
        currentIndex++;

        // إذا وصلنا لنهاية القصة
       if (currentIndex >= 8)
{
    Debug.Log("Story Completed");

    if (endButton != null)
    {
        endButton.SetActive(true);        // Make it visible
        Button btn = endButton.GetComponent<Button>();
        if (btn != null)
            btn.interactable = true;      // Make it clickable
    }

    return;
}

        // بداية الصفحة الثانية
        if (currentIndex == transitionIndex)
        {
            StartCoroutine(ClearPreviousPage());
        }

        panels[currentIndex].SetActive(true);
        StartCoroutine(FadeIn(panels[currentIndex].GetComponent<Image>()));
    }

    IEnumerator ClearPreviousPage()
    {
        for (int i = 0; i < transitionIndex; i++)
        {
            StartCoroutine(FadeOut(panels[i].GetComponent<Image>()));
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FadeIn(Image img)
    {
        float alpha = 0;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 2;
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }
    }

    IEnumerator FadeOut(Image img)
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 2;
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
            yield return null;
        }
        img.gameObject.SetActive(false);
    }
}