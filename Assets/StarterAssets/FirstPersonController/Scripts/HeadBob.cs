using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets; // D˘leûitÈ: MusÌme vidÏt FirstPersonController

public class HeadBob : MonoBehaviour
{
    [Header("PropojenÌ")]
    public FirstPersonController fpsController; // Odkaz na hlavnÌ skript

    [Header("Ch˘ze (Walk)")]
    public float walkSpeed = 14f;
    public float walkAmountY = 0.05f;
    public float walkAmountX = 0.03f;

    [Header("BÏh (Sprint)")]
    public float sprintSpeed = 20f;
    public float sprintAmountY = 0.1f;
    public float sprintAmountX = 0.06f;

    [Header("Efekty")]
    public float smoothTime = 10f;
    [Range(0, 3)] public float tiltIntensity = 1.5f; // SÌla n·klonu

    private float defaultPosY;
    private float defaultPosX;
    private float timer = 0;

    // Lerp promÏnnÈ
    private float currentSpeed;
    private float currentAmountY;
    private float currentAmountX;

    void Start()
    {
        defaultPosY = transform.localPosition.y;
        defaultPosX = transform.localPosition.x;

        // Pokud nem·me odkaz, najdeme ho na rodiËi (PlayerCapsule)
        if (fpsController == null)
            fpsController = GetComponentInParent<FirstPersonController>();
    }

    void Update()
    {
        if (fpsController == null) return;

        // Rychlost zjiöùujeme p¯Ìmo z controlleru
        float speed = new Vector3(fpsController.GetComponent<CharacterController>().velocity.x, 0, fpsController.GetComponent<CharacterController>().velocity.z).magnitude;
        bool isGrounded = fpsController.Grounded;

        // Detekce sprintu
        bool isSprinting = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;

        // CÌlovÈ hodnoty
        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
        float targetAmountY = isSprinting ? sprintAmountY : walkAmountY;
        float targetAmountX = isSprinting ? sprintAmountX : walkAmountX;

        // Plynul˝ p¯echod
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * 5f);
        currentAmountY = Mathf.Lerp(currentAmountY, targetAmountY, Time.deltaTime * 5f);
        currentAmountX = Mathf.Lerp(currentAmountX, targetAmountX, Time.deltaTime * 5f);

        if (speed > 0.1f && isGrounded)
        {
            timer += Time.deltaTime * currentSpeed;

            // 1. POZICE (H˝beme p¯Ìmo tÌmto objektem - PlayerCameraRoot)
            // To nevadÌ, FirstPersonController pozici nep¯episuje, jen rotaci.
            float newY = defaultPosY + Mathf.Sin(timer) * currentAmountY;
            float newX = defaultPosX + Mathf.Cos(timer / 2) * currentAmountX;
            transform.localPosition = new Vector3(newX, newY, transform.localPosition.z);

            // 2. ROTACE (N·klon)
            // Tady to posÌl·me do FirstPersonControlleru!
            float tilt = -Mathf.Cos(timer / 2) * tiltIntensity;
            fpsController.OverrideTilt = tilt; // Zapisujeme do novÈ promÏnnÈ
        }
        else
        {
            // Reset do klidu
            timer = 0;

            // N·vrat pozice
            Vector3 targetPos = new Vector3(defaultPosX, defaultPosY, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothTime);

            // N·vrat n·klonu na nulu (p¯es controller)
            fpsController.OverrideTilt = Mathf.Lerp(fpsController.OverrideTilt, 0f, Time.deltaTime * smoothTime);
        }
    }
}