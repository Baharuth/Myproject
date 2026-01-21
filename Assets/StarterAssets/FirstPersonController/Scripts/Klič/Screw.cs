using UnityEngine;

public class Screw : MonoBehaviour
{
    [Header("Nastavení Šroubu")]
    public float rotationSpeed = 100f; // Rychlost otáèení
    public float maxRotation = 360f;   // Kolik stupòù je potøeba k vyšroubování (napø. 3 otoèky = 1080)
    public Vector3 rotationAxis = Vector3.forward; // Osa otáèení (Z = forward, Y = up, X = right)

    [Header("Stav")]
    public float currentRotation = 0f;
    public bool isUnscrewed = false;

    // Tuto metodu bude volat Wrench, když na šroub míøíš a držíš tlaèítko
    public void RotateScrew()
    {
        if (isUnscrewed) return; // Pokud je vyšroubovaný, už nic nedìláme

        // Vypoèítáme otoèku pro tento snímek
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Aplikujeme rotaci na objekt
        transform.Rotate(rotationAxis * rotationAmount);

        // Pøièteme k celkovému poètu otoèení
        currentRotation += rotationAmount;

        // Kontrola, jestli je hotovo
        if (currentRotation >= maxRotation)
        {
            UnscrewComplete();
        }
    }

    void UnscrewComplete()
    {
        isUnscrewed = true;
        Debug.Log("<color=green>Šroub vyšroubován!</color>");

        // Zde mùžeš pøidat logiku co se stane (napø. šroub spadne na zem)
        // Pøíklad: Zapneme fyziku aby odpadl
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        else
        {
            // Nebo ho prostì znièíme/schováme
            gameObject.SetActive(false);
        }
    }
}