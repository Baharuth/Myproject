using UnityEngine;
using Cinemachine;
using StarterAssets;

public class SprintFOV : MonoBehaviour
{
    [Header("Nastavení")]
    public float sprintFOV = 60f; // Cílové FOV pøi sprintu
    public float changeSpeed = 5f; // Rychlost zmìny

    [Header("Debug")]
    public bool showDebugLogs = true; // Zaškrtni pro vypisování do konzole

    [Header("Odkazy")]
    public FirstPersonController playerController;
    public CinemachineVirtualCamera virtualCamera;

    private float defaultFOV;
    private CharacterController charController;
    private bool wasSprinting = false; // Pomocná promìnná pro detekci zmìny stavu

    void Start()
    {
        // 1. INICIALIZACE A KONTROLA
        if (playerController != null)
        {
            charController = playerController.GetComponent<CharacterController>();
            if (showDebugLogs) Debug.Log("<color=green>[SprintFOV] START:</color> Hráè nalezen.");
        }
        else
        {
            Debug.LogError("<color=red>[SprintFOV] CHYBA:</color> Chybí odkaz na Player Controller!");
        }

        if (virtualCamera != null)
        {
            defaultFOV = virtualCamera.m_Lens.FieldOfView;
            if (showDebugLogs) Debug.Log($"<color=green>[SprintFOV] START:</color> Kamera nalezena. Základní FOV uloženo: {defaultFOV}");
        }
        else
        {
            Debug.LogError("<color=red>[SprintFOV] CHYBA:</color> Chybí odkaz na Virtual Camera!");
        }
    }

    void Update()
    {
        if (playerController == null || virtualCamera == null || charController == null) return;

        // 2. VÝPOÈET RYCHLOSTI (ignorujeme osu Y - skákání)
        float currentSpeed = new Vector3(charController.velocity.x, 0f, charController.velocity.z).magnitude;

        // 3. LOGIKA SPRINTU
        // Pokud je rychlost vyšší než bìžná chùze + malá rezerva (0.1f)
        bool isSprinting = currentSpeed > (playerController.MoveSpeed + 0.1f);

        // Cílové FOV
        float targetFOV = isSprinting ? sprintFOV : defaultFOV;

        // 4. APLIKACE ZMÌNY (Lerp)
        virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(virtualCamera.m_Lens.FieldOfView, targetFOV, Time.deltaTime * changeSpeed);

        // 5. CHYTRÝ DEBUGGING (Vypíše jen pøi zmìnì stavu)
        if (showDebugLogs)
        {
            if (isSprinting && !wasSprinting)
            {
                // Právì jsi zaèal sprintovat
                Debug.Log($"<color=cyan>[SprintFOV] SPRINT AKTIVNÍ!</color> Rychlost: {currentSpeed:F1} (Hranice: {playerController.MoveSpeed}). Mìním FOV na {sprintFOV}.");
            }
            else if (!isSprinting && wasSprinting)
            {
                // Právì jsi pøestal sprintovat
                Debug.Log($"<color=orange>[SprintFOV] SPRINT UKONÈEN.</color> Rychlost klesla na: {currentSpeed:F1}. Vracím FOV na {defaultFOV}.");
            }
        }

        // Uložíme aktuální stav pro pøíští snímek
        wasSprinting = isSprinting;
    }
}