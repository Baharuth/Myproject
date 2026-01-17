using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    public float damagePerSecond = 10f; // Kolik HP ubere za sekundu
    private ParticleSystem ps;

    void Start()
    {
        // Najdeme Particle System na tomto objektu
        ps = GetComponent<ParticleSystem>();
    }

    // Tato metoda bìží celou dobu, co je nìkdo uvnitø tvého Box Collideru
    private void OnTriggerStay(Collider other)
    {
        // 1. KONTROLA: Støíká hasièák? (Pokud ne, nedáváme damage)
        if (ps != null && ps.isPlaying)
        {
            // 2. KONTROLA: Má to, co je v collideru, skript Health?
            Health health = other.GetComponent<Health>();

            if (health != null)
            {
                // Výpoèet damage za snímek (aby to nebylo moc rychlé)
                float frameDamage = damagePerSecond * Time.deltaTime;
                health.TakeDamage(frameDamage);

                Debug.Log("Box Collider detekoval cíl: " + other.name + " | Ubírám HP");
            }
        }
    }
}