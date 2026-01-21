using UnityEngine;
using UnityEngine.InputSystem; // Potøeba pro ovládání myší

public class Wrench : MonoBehaviour
{
    [Header("Nastavení")]
    public float reachDistance = 3f; // Jak daleko klíè dosáhne
    public LayerMask screwLayer;     // Vrstva, na které jsou šrouby (vìtšinou Default)

    private Item itemScript;
    private Camera mainCam;

    void Start()
    {
        itemScript = GetComponent<Item>();
        mainCam = Camera.main; // Najdeme hlavní kameru pro Raycast
    }

    void Update()
    {
        // 1. Kontrola: Máme klíè vùbec v ruce? (pomocí tvého skriptu Item)
        if (itemScript == null || !itemScript.isPickedUp) return;

        // 2. Kontrola: Drží hráè levé tlaèítko myši?
        if (Mouse.current.leftButton.isPressed)
        {
            UseWrench();
        }
    }

    void UseWrench()
    {
        RaycastHit hit;

        // Støílíme paprsek ze støedu obrazovky (z kamery)
        // itemScript.isPickedUp nám zaruèuje, že ho držíme, takže støílíme z oèí hráèe
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, reachDistance, screwLayer))
        {
            // Zkusíme najít na trefeném objektu skript Screw
            Screw screw = hit.collider.GetComponent<Screw>();

            if (screw != null)
            {
                // Zavoláme metodu otáèení na šroubu
                screw.RotateScrew();

                // Volitelné: Zde mùžeš pøidat zvuk vrzání nebo animaci klíèe
            }
        }
    }
}