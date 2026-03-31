using UnityEngine;

public class WaterWell3DScript : MonoBehaviour
{
    public AudioClip fillWaterSound;
    public ParticleSystem waterSplashEffect;

    private bool hasFilled = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !hasFilled)
        {
            // 1️⃣ تأكد من اختيار الدلو الفاضي
            if (AdvancedToolManager.Instance != null &&
                AdvancedToolManager.currentToolName == "EmptyGardel" &&
                !AdvancedToolManager.Instance.isGardelFull)
            {
                FillGardel();
                hasFilled = true;
            }
            else
            {
                // رسالة تساعد اللاعب يعرف أنه يحتاج اختيار الدلو
                Debug.Log("اختر الدلو الفاضي أولاً لتعبئته!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasFilled = false; // يرجع يسمح بالتعبئة مرة ثانية
        }
    }

    private void FillGardel()
    {
        // 2️⃣ تشغيل الصوت
        if (fillWaterSound != null)
            AudioSource.PlayClipAtPoint(fillWaterSound, transform.position);

        // 3️⃣ تأثير الماء
        if (waterSplashEffect != null)
        {
            ParticleSystem splash = Instantiate(waterSplashEffect, transform.position, Quaternion.identity);
            Destroy(splash.gameObject, 2f);
        }

        // 4️⃣ تحديث الدلو في اليد
        AdvancedToolManager.Instance.FillGardel();

        // 5️⃣ توتوريال
        if (TutorialManager.Instance != null)
            TutorialManager.Instance.CompleteAction(TutorialManager.TutorialStep.FullWater);

        Debug.Log("تم تعبئة الدلو ✅");
    }
}