using UnityEngine;

public class UpDownMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float distance = 5f; 
    public float speed = 2f; 
    public bool reverseDirection = false; 
    
    private Vector3 startPosition;

    void Start()
    {
        // حفظ موقع البداية عند تشغيل اللعبة
        startPosition = transform.position;
    }

    void Update()
    {
        // حساب الحركة الترددية
        float movement = Mathf.PingPong(Time.time * speed, distance);
        
        // عكس القيمة إذا كان الخيار مفعلاً
        float finalMovement = reverseDirection ? -movement : movement;

        // تطبيق الحركة على محور Y (للأعلى والأسفل)
        transform.position = startPosition + new Vector3(0, finalMovement, 0);
    }
}