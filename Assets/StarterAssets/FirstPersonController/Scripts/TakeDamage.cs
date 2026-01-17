using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // Podíváme se, jestli to, co do pasti vlezlo, má skript Health
        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}