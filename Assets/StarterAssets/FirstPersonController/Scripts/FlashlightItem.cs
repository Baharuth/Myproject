using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Item))] // Tento skript vyžaduje, aby na objektu byl i skript Item
public class FlashlightItem : MonoBehaviour
{
    [Header("Nastavení")]
    public GameObject LightSource; // Sem pøetáhneš ten Spotlight (dítì objektu)
    public AudioSource Audio;      // (Volitelné)
    public AudioClip ClickSound;   // (Volitelné)

    private Item _itemScript;
    private bool _isOn = false;

    private void Start()
    {
        // Najdeme skript Item na stejném objektu
        _itemScript = GetComponent<Item>();

        // Na zaèátku svìtlo vypneme (nebo zapneme podle _isOn)
        if (LightSource != null)
            LightSource.SetActive(_isOn);
    }

    private void Update()
    {
        // Svìtlo jde ovládat JENOM, když je pøedmìt sebraný (isPickedUp == true)
        if (_itemScript != null && _itemScript.isPickedUp)
        {
            // Kontrola Levé myši (Left Button)
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                ToggleFlashlight();
            }
        }
    }

    private void ToggleFlashlight()
    {
        _isOn = !_isOn;

        if (LightSource != null)
        {
            LightSource.SetActive(_isOn);
        }

        if (Audio != null && ClickSound != null)
        {
            Audio.PlayOneShot(ClickSound);
        }
    }
}