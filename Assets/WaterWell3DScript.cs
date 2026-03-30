using UnityEngine;

public class WaterWell3DScript : MonoBehaviour
{
    
    public AudioClip fillWaterSound;
    public ParticleSystem waterSplashEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 👇 تحقق أن الجردل مختار + فاضي
            if (AdvancedToolManager.Instance != null &&
                AdvancedToolManager.currentToolName == "EmptyGardel" &&
                !AdvancedToolManager.Instance.isGardelFull)
            {
                FillGardel();
            }
        }
    }

    private void FillGardel()
    {
        if (fillWaterSound != null)
            AudioSource.PlayClipAtPoint(fillWaterSound, transform.position);

        if (waterSplashEffect != null)
        {
            ParticleSystem splash = Instantiate(waterSplashEffect, transform.position, Quaternion.identity);
            Destroy(splash.gameObject, 2f);
        }

        AdvancedToolManager.Instance.FillGardel();

        if (TutorialManager.Instance != null)
            TutorialManager.Instance.CompleteAction(TutorialManager.TutorialStep.FullWater);

        Debug.Log("تم تعبئة الماء ✅");
    }

}
