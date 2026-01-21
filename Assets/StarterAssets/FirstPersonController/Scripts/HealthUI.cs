using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health playerHealth;
    public Slider healthSlider;

    // Pomocná promìnná, abychom nevypisovali do konzole to samé poøád dokola
    private float lastHealthValue = -1f;

    void Start()
    {
        // 1. KONTROLA PO SPUŠTÌNÍ: Jsou vìci pøipojené?
        if (playerHealth != null)
        {
            Debug.Log("<color=green>HealthUI:</color> Hráè (skript Health) byl úspìšnì nalezen!");
        }
        else
        {
            Debug.LogError("<color=red>HealthUI CHYBA:</color> Chybí odkaz na 'Player Health'! Pøetáhni hráèe do skriptu v Inspectoru.");
        }

        if (healthSlider != null)
        {
            Debug.Log("<color=green>HealthUI:</color> Slider (UI) byl úspìšnì nalezen!");
        }
        else
        {
            Debug.LogError("<color=red>HealthUI CHYBA:</color> Chybí odkaz na 'Health Slider'! Pøetáhni Slider do skriptu v Inspectoru.");
        }
    }

    void Update()
    {
        if (playerHealth != null && healthSlider != null)
        {
            // Výpoèet procenta (napø. 0.8 pro 80%)
            float hpPercent = playerHealth.currentHealth / playerHealth.maxHealth;

            // Nastavení slideru
            healthSlider.value = hpPercent;

            // 2. KONTROLA ZA BÌHU: Vypíšeme info jen tehdy, když se zmìní životy
            if (playerHealth.currentHealth != lastHealthValue)
            {
                Debug.Log($"<color=yellow>Zmìna UI:</color> Aktuální zdraví: {playerHealth.currentHealth} / {playerHealth.maxHealth} (Slider nastaven na: {hpPercent})");

                // Uložíme si aktuální hodnotu, abychom ji v pøíštím snímku nevypisovali znovu
                lastHealthValue = playerHealth.currentHealth;
            }
        }
    }
}