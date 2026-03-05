using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    public GameObject[] panels; 
    public int transitionIndex = 4;

    public bool storyCompleted = false; // لمعرفة هل انتهت القصة

    private int currentIndex = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !storyCompleted)
        {
            ShowNext();
        }
    }

    void ShowNext()
    {
        currentIndex++;

        // إذا وصلنا لنهاية القصة
        if (currentIndex >= panels.Length)
        {
            storyCompleted = true;
            Debug.Log("Story Completed");
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