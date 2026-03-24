using UnityEngine;

public class WoodItem : MonoBehaviour
{
    public int value = 1;

    // سنستخدم هذه الدالة للتأكد من التلامس
    private void OnTriggerEnter(Collider other)
    {
        // اطبع اسم الشيء الذي لمس الخشب للتأكد
        Debug.Log("شيء ما لمس الخشب واسمه: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            if (ResourceManager.instance != null)
            {
                ResourceManager.instance.AddWood(value);
                Debug.Log("تمت إضافة الخشب للعداد!");
            }
            Destroy(gameObject); // حذف الخشب فوراً
        }
    }
}