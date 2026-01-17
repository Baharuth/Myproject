using UnityEngine;

public class FireSource : MonoBehaviour
{
    public ParticleSystem fireParticles;
    public float fireHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = fireHealth;
    }

    // TATO METODA JE PRO ÈÁSTICE NEJSPOLEHLIVÌJŠÍ
    private void OnParticleCollision(GameObject other)
    {
        // Debug, abychom vidìli, že èástice do nìèeho narazila
        Debug.Log("<color=cyan>Kolize:</color> Èástice z " + other.name + " zasáhla oheò!");

        ReduceFire(1.0f);
    }

    public void ReduceFire(float amount)
    {
        currentHealth -= amount;
        Debug.Log("<color=orange>Oheò HP:</color> " + currentHealth);

        if (fireParticles != null)
        {
            var emission = fireParticles.emission;
            emission.rateOverTime = (currentHealth / fireHealth) * 40f;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("<color=green>Oheò uhašen!</color>");
            if (fireParticles != null) fireParticles.Stop();
            gameObject.SetActive(false); // Oheò zmizí úplnì
        }
    }
}