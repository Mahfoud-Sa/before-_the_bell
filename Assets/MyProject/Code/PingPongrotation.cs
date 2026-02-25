using UnityEngine;

public class SquarePathMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float distance = 5f;  
    public float speed = 2f;     

    private Vector3 startPosition;
    private int phase = 0; // المراحل: 0=Z+, 1=Y+, 2=X-, 3=Y-
    private float progress = 0f; // تتبع المسافة المقطوعة في المرحلة الحالية

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // زيادة التقدم بناءً على السرعة والزمن
        progress += Time.deltaTime * speed;

        Vector3 currentPos = transform.position;

        if (phase == 0) // 1. التحرك لليمين على محور Z
        {
            transform.position = startPosition + new Vector3(0, 0, progress);
            if (progress >= distance) SwitchToNextPhase();
        }
        else if (phase == 1) // 2. التحرك للأعلى على محور Y
        {
            transform.position = startPosition + new Vector3(0, progress, distance);
            if (progress >= distance) SwitchToNextPhase();
        }
        else if (phase == 2) // 3. التحرك لليسار على محور X
        {
            transform.position = startPosition + new Vector3(-progress, distance, distance);
            if (progress >= distance) SwitchToNextPhase();
        }
        else if (phase == 3) // 4. النزول للأسفل (العودة لنقطة البداية تدريجياً)
        {
            // هنا نقوم بإنقاص Y والعودة ببقية المحاور تدريجياً إذا أردت إغلاق المربع
            transform.position = startPosition + new Vector3(-distance, distance - progress, distance);
            if (progress >= distance) SwitchToNextPhase();
        }
    }

    void SwitchToNextPhase()
    {
        progress = 0; // تصغير العداد للمرحلة القادمة
        phase = (phase + 1) % 4; // الانتقال للمرحلة التالية (من 0 إلى 3)
        
        // تحديث startPosition ليكون نقطة الارتكاز الجديدة إذا كنت تريد مساراً مفتوحاً
        // لكن في هذا الكود ثبتنا startPosition لنرسم مربعاً كاملاً حولها.
    }
}