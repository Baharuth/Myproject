using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Nastavení zdraví")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Stav")]
    public bool isDead = false;

    void Start()
    {
        // Na zaèátku hry nastavíme zdraví na maximum
        currentHealth = maxHealth;

        // POTVRZENÍ DO KONZOLE
        Debug.Log("<color=cyan>Health System:</color> Inicializace dokonèena na objektu: <b>" + gameObject.name + "</b>. Zdraví nastaveno na: " + currentHealth);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("<color=orange>Poškození:</color> " + gameObject.name + " ztratil " + amount + " HP. Zbývá: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("<color=green>Léèení:</color> " + gameObject.name + " se vyléèil o " + amount + ". Aktuální HP: " + currentHealth);
    }

    void Die()
    {
        isDead = true;
        currentHealth = 0;
        Debug.Log("<color=red>SMRT:</color> " + gameObject.name + " zemøel!");

        // Sem mùžeš pøidat Destroy(gameObject) nebo restart levelu
    }
}