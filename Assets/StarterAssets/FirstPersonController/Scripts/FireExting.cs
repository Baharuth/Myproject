using UnityEngine;
using UnityEngine.InputSystem;

public class FireExting : MonoBehaviour
{
    [Header("Reference")]
    public ParticleSystem foamParticles;
    public float extinguishPower = 1f; // Síla hašení jedné èástice
    private Item itemScript;

    void Start()
    {
        itemScript = GetComponent<Item>();
        if (foamParticles != null) foamParticles.Stop();
    }

    void Update()
    {
        if (itemScript == null) return;

        if (itemScript.isPickedUp && Mouse.current.leftButton.isPressed)
        {
            if (!foamParticles.isPlaying)
            {
                foamParticles.Play();
                Debug.Log("Hasièák støíká!");
            }
        }
        else
        {
            if (foamParticles.isPlaying)
            {
                foamParticles.Stop();
                Debug.Log("Hasièák zastaven.");
            }
        }
    }

    // NOVÁ ÈÁST: Detekce nárazu pìny do ohnì
    private void OnParticleCollision(GameObject other)
    {
        // Zkontrolujeme, zda jsme zasáhli nìco, co má skript FireSource
        FireSource fire = other.GetComponent<FireSource>();
        if (fire != null)
        {
            fire.ReduceFire(extinguishPower);
        }
    }
}