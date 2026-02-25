using UnityEngine;

public class PalmBridge : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasFallen = false;
    private bool isPlayerTouching = false; 

    [Header("Settings")]
    public KeyCode cutKey = KeyCode.E;    
    public Transform player;              

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // التأكد من أن الشجرة ثابتة في البداية
        rb.isKinematic = true; 
        rb.useGravity = true; 
    }

    void Update()
    {
        // إذا كان اللاعب يلمس هذه الشجرة تحديداً ولم تسقط بعد وضغط E
        if (isPlayerTouching && !hasFallen && Input.GetKeyDown(cutKey))
        {
            StartFalling();
        }
    }

    // دالة واحدة شاملة لكل التصادمات لمنع الأخطاء
    private void OnCollisionEnter(Collision collision)
    {
        // التحقق هل المصطدم هو اللاعب (عن طريق الـ Tag أو الاسم)
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.name == "Player")
        {
            isPlayerTouching = true;
            Debug.Log("اللاعب يلمس: " + gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // عندما يبتعد اللاعب عن الشجرة
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.name == "Player")
        {
            isPlayerTouching = false;
            Debug.Log("اللاعب ابتعد عن: " + gameObject.name);
        }
    }

    void StartFalling()
    {
        hasFallen = true;
        rb.isKinematic = false; // تفعيل الفيزياء للسقوط

        // فك كل القيود لكي تسقط بشكل طبيعي
        rb.constraints = RigidbodyConstraints.None;

        // دفع الشجرة بعيداً عن اتجاه اللاعب (دفعة واقعية)
        Vector3 pushDirection = (transform.position - player.position).normalized;
        rb.AddForce(pushDirection * 7f, ForceMode.Impulse);
        
        // إضافة حركة دورانية خفيفة لضمان السقوط
        rb.AddTorque(transform.right * 12f, ForceMode.Impulse); 

        Debug.Log("تم قطع الشجرة بنجاح!");
    }
}