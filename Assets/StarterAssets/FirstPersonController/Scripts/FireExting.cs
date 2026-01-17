using UnityEngine;
using UnityEngine.InputSystem; // Nutné pro snímání myši

public class FireExting : MonoBehaviour
{
    [Header("Reference")]
    public ParticleSystem foamParticles; // Pøetáhni sem svùj Particle System
    private Item itemScript;

    void Start()
    {
        // Najdeme skript Item, který už na hasièáku máš
        itemScript = GetComponent<Item>();

        // Na zaèátku èástice vypneme
        if (foamParticles != null)
        {
            foamParticles.Stop();
        }
    }

    void Update()
    {
        if (itemScript == null) return;

        // PODMÍNKA: Je zvednutý A hráè drží levé tlaèítko myši
        if (itemScript.isPickedUp && Mouse.current.leftButton.isPressed)
        {
            // Pokud èástice ještì nebìží, zapneme je
            if (!foamParticles.isPlaying)
            {
                foamParticles.Play();
                Debug.Log("Hasièák støíká!");
            }
        }
        else
        {
            // Pokud hráè pustí tlaèítko nebo hasièák zahodí, zastavíme èástice
            if (foamParticles.isPlaying)
            {
                foamParticles.Stop();
                Debug.Log("Hasièák zastaven.");
            }
        }
    }
}