using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    public GameObject[] panels; // اسحب كل الصور هنا (صفحة 1 ثم صفحة 2)
    public int transitionIndex = 4; // رقم الصورة التي عندها تنتهي الصفحة الأولى (مثلاً 4)
    // public string nextSceneName; 
    
    private int currentIndex = 0; // نبدأ من -1 ليظهر العنصر الأول عند أول ضغطة

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

        if (currentIndex < panels.Length)
        {
            // إذا وصلنا للصورة الخامسة (بداية الصفحة الثانية)
            if (currentIndex == transitionIndex)
            {
                StartCoroutine(ClearPreviousPage());
            }
            
            // إظهار الصورة الحالية بتأثير Fade In بسيط
            panels[currentIndex].SetActive(true);
            StartCoroutine(FadeIn(panels[currentIndex].GetComponent<Image>()));
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    // كود إخفاء الصفحة الأولى بالتدريج
    IEnumerator ClearPreviousPage()
    {
        for (int i = 0; i < transitionIndex; i++)
        {
            StartCoroutine(FadeOut(panels[i].GetComponent<Image>()));
            yield return new WaitForSeconds(0.2f); // تأخير بسيط بين اختفاء كل صورة وأخرى
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