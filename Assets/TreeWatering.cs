using UnityEngine;

public class TreeWatering : MonoBehaviour
{
    public Sprite dryTreeSprite;     // الشجرة اليابسة
    public Sprite greenTreeSprite;   // الشجرة الخضراء

    private SpriteRenderer spriteRenderer;
    private bool isWatered = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = dryTreeSprite;
    }

    private void OnMouseDown()
    {
        // 👇 يتأكد إن الشجرة لسه ما تسقت
        if (isWatered) return;

        // 👇 يتأكد إن اللاعب معه دلو مليان
        if (AdvancedToolManager.Instance != null &&
            AdvancedToolManager.currentToolName == "FullGardel")
        {
            WaterTree();
        }
        else
        {
            Debug.Log("لازم يكون معك دلو مليان 💧");
        }
    }

    void WaterTree()
    {
        isWatered = true;

        // تغيير شكل الشجرة
        spriteRenderer.sprite = greenTreeSprite;

        // 👇 يرجع الدلو فاضي بعد الاستخدام
        AdvancedToolManager.Instance.ResetGardelToEmpty();


        Debug.Log("تم سقي الشجرة 🌳✅");
    }

}
