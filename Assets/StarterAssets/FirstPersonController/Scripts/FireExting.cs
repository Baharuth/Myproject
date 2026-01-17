using UnityEngine;
using UnityEngine.InputSystem;

public class FireExting : MonoBehaviour
{
    public ParticleSystem foamParticles;
    public float extinguishPower = 20f; // Síla hašení za sekundu
    private Item itemScript;

    void Start()
    {
        itemScript = GetComponent<Item>();
    }

    void Update()
    {
        if (itemScript == null) return;

        bool isSpraying = itemScript.isPickedUp && Mouse.current.leftButton.isPressed;

        if (isSpraying)
        {
            if (!foamParticles.isPlaying) foamParticles.Play();

            // RUÈNÍ DETEKCE: Vystøelíme neviditelný paprsek (Raycast)
            // nebo zkontrolujeme, co je pøed námi
            ShootExtinguishRay();
        }
        else
        {
            if (foamParticles.isPlaying) foamParticles.Stop();
        }
    }

    void ShootExtinguishRay()
    {
        RaycastHit hit;
        // Støílíme dopøedu z hasièáku do vzdálenosti 5 metrù
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            FireSource fire = hit.collider.GetComponent<FireSource>();
            if (fire != null)
            {
                // Hasíme pøímo pøes metodu v ohni
                fire.ReduceFire(extinguishPower * Time.deltaTime);
                Debug.Log("Raycast trefil oheò!");
            }
        }
    }
}