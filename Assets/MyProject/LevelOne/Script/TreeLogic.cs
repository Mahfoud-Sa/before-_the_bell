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
    rb.isKinematic = false; // تفعيل الفيزياء

    // 1. قفل الدوران في المحاور الجانبية لضمان سقوط مستقيم للأمام
    // هذا يمنع الشجرة من الميلان يميناً أو يساراً أثناء السقوط
    rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    // 2. الدفع للأمام (اتجاه السهم الأزرق للشجرة)
    // نستخدم transform.forward ليكون السقوط دائماً للأمام بغض النظر عن مكان اللاعب
    rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
    
    // 3. إضافة عزم دوران (Torque) لثني الشجرة من القاعدة للأمام
    rb.AddTorque(transform.right * 15f, ForceMode.Impulse); 

    Debug.Log("تم القطع والسقوط للأمام باتجاه السيل!");
    }

}