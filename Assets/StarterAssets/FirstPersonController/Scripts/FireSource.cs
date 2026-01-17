using UnityEngine;

public class FireSource : MonoBehaviour
{
    public ParticleSystem fireParticles; // Pøetáhni sem èástice ohnì
    public float fireHealth = 100f;      // "Životy" ohnì
    public float extinguishRate = 20f;   // Jak rychle oheò uhasne

    public void ReduceFire(float amount)
    {
        fireHealth -= amount;

        // Pokud oheò slábne, mùžeme zmenšit emisi èástic (dobrovolné)
        var emission = fireParticles.emission;
        emission.rateOverTime = (fireHealth / 100f) * 50f; // Pøíklad zmenšování

        if (fireHealth <= 0)
        {
            FinishExtinguishing();
        }
    }

    void FinishExtinguishing()
    {
        fireParticles.Stop();
        Debug.Log("Oheò byl uhašen!");
        // Volitelnì: Destroy(gameObject, 2f);
    }
}