using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class FlashlightSystem : MonoBehaviour
{
    [Header("Nastavení")]
    [Tooltip("Sem pøetáhni ten Spotlight, který jsi vytvoøil")]
    public GameObject LightSource;

    [Tooltip("Má být baterka zapnutá hned pøi startu hry?")]
    public bool IsOn = false;

    [Header("Audio (Volitelné)")]
    [Tooltip("Sem mùžeš dát AudioSource pro zvuk cvaknutí")]
    public AudioSource Audio;
    [Tooltip("Zvuk cvaknutí vypínaèe")]
    public AudioClip ClickSound;

    private void Start()
    {
        // Na zaèátku nastavíme svìtlo podle toho, jak chceme (vypnuté/zapnuté)
        if (LightSource != null)
        {
            LightSource.SetActive(IsOn);
        }
    }

    private void Update()
    {
        // Kontrola stisknutí klávesy F
        if (WasTogglePressed())
        {
            ToggleFlashlight();
        }
    }

    private bool WasTogglePressed()
    {
        // 1. Zkusíme nový Input System (klávesnice)
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            return true;
        }
        // 2. Zkusíme Gamepad (tøeba tlaèítko D-Pad Up - šipka nahoru)
        if (Gamepad.current != null && Gamepad.current.dpad.up.wasPressedThisFrame)
        {
            return true;
        }
#endif
        // 3. Záchrana pro starý systém (kdyby náhodou)
        if (Input.GetKeyDown(KeyCode.F))
        {
            return true;
        }

        return false;
    }

    private void ToggleFlashlight()
    {
        // Prohodíme stav (zapnuto -> vypnuto a naopak)
        IsOn = !IsOn;

        // Aplikujeme na objekt svìtla
        if (LightSource != null)
        {
            LightSource.SetActive(IsOn);
        }

        // Pøehrajeme zvuk, pokud je nastavený
        if (Audio != null && ClickSound != null)
        {
            Audio.PlayOneShot(ClickSound);
        }
    }
}