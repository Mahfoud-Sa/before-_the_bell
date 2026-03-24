using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // Maximum health points
    public int currentHealth;
    public GameObject gameOverObject; // GameObject to show when health is 0
    public Image[] healthImages; // Array to store health UI images
    private SpriteRenderer spriteRenderer;
    private bool isInvincible = false; // Prevent multiple damage during flicker

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameOverObject.SetActive(false); // Ensure game over object is hidden at the start
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < healthImages.Length; i++)
        {
            if (i < currentHealth)
                healthImages[i].enabled = true; // Show health images
            else
                healthImages[i].enabled = false; // Hide health images
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap") && !isInvincible)
        {
            TakeDamage(1);
            StartCoroutine(FlickerEffect());
        }

    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            gameOverObject.SetActive(true); // Show the hidden game object when health is 0
            Time.timeScale = 0;
            // Additional game over logic here
        }
    }

    IEnumerator FlickerEffect()
    {
        isInvincible = true;

        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }

        isInvincible = false;
    }
}
