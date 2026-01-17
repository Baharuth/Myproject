using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public Image healthBar; // musí být Image typu Filled

    void Update()
    {
        if (healthBar != null)
            healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0f, 1f);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health < 0) health = 0;
    }
}