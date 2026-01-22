using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Nutné pro TextMeshPro

public class Pickup : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject pickupMessageUI; // Panel pozadí (volitelné)
    public TextMeshProUGUI pickupText; // Text: "Zmáčkni F..."
    public TextMeshProUGUI heldItemText; // NOVÉ: Text: "Držíš: <Jméno>"

    [Header("Settings")]
    public Transform holdArea;
    public Key pickupKey = Key.F;
    public float pickupRange = 5f;
    public float dropForce = 5f;
    public LayerMask pickupLayer;
    public float followSpeed = 10f;

    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private Collider heldObjCol;

    private GameObject currentTargetObj;

    void Start()
    {
        // Na začátku vše skryjeme
        if (pickupMessageUI != null) pickupMessageUI.SetActive(false);
        if (pickupText != null) pickupText.gameObject.SetActive(false);
        if (heldItemText != null) heldItemText.gameObject.SetActive(false); // Skryjeme text drženého předmětu
    }

    void Update()
    {
        // 1. Pokud už něco držíme
        if (heldObj != null)
        {
            FollowHand();

            // Ujistíme se, že nápověda "Zmáčkni F" je pryč
            if (pickupMessageUI != null) pickupMessageUI.SetActive(false);
            if (pickupText != null) pickupText.gameObject.SetActive(false);

            currentTargetObj = null;

            // Položení
            if (Keyboard.current != null && Keyboard.current[pickupKey].wasPressedThisFrame)
            {
                DropObject();
            }
        }
        else
        {
            // 2. Pokud nic nedržíme
            DetectObjectInFront();

            // Zvednutí
            if (Keyboard.current != null && Keyboard.current[pickupKey].wasPressedThisFrame)
            {
                if (currentTargetObj != null)
                {
                    PerformPickup(currentTargetObj);
                }
            }
        }
    }

    void DetectObjectInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, pickupLayer))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item == null) item = hit.collider.GetComponentInParent<Item>();
            if (item == null) item = hit.collider.GetComponentInChildren<Item>();

            if (item != null)
            {
                currentTargetObj = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;

                // Zobrazíme nápovědu pro sebrání
                if (pickupMessageUI != null) pickupMessageUI.SetActive(true);
                if (pickupText != null)
                {
                    pickupText.text = "Zmáčkni [F] pro sebrání: " + item.itemName;
                    pickupText.gameObject.SetActive(true);
                }
                return;
            }
        }

        currentTargetObj = null;
        if (pickupMessageUI != null) pickupMessageUI.SetActive(false);
        if (pickupText != null) pickupText.gameObject.SetActive(false);
    }

    void PerformPickup(GameObject target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (!rb) return;

        heldObj = target;
        heldObjRb = rb;
        heldObjCol = heldObj.GetComponent<Collider>();

        // Získání informací o itemu
        Item item = heldObj.GetComponent<Item>();
        if (item == null) item = heldObj.GetComponentInParent<Item>();
        if (item == null) item = heldObj.GetComponentInChildren<Item>();

        if (item != null)
        {
            item.isPickedUp = true;

            // --- ZDE JE NOVÁ ČÁST PRO TEXT DRŽENÉHO PŘEDMĚTU ---
            if (heldItemText != null)
            {
                heldItemText.text = "Držíš: " + item.itemName;
                heldItemText.gameObject.SetActive(true); // Zapneme text "Držíš..."
            }
        }

        heldObjRb.isKinematic = true;
        if (heldObjCol) heldObjCol.enabled = false;
    }

    void DropObject()
    {
        Item item = heldObj.GetComponent<Item>();
        if (item == null) item = heldObj.GetComponentInParent<Item>();
        if (item == null) item = heldObj.GetComponentInChildren<Item>();

        if (item != null)
        {
            item.isPickedUp = false;
        }

        // --- SKRYJEME TEXT DRŽENÉHO PŘEDMĚTU ---
        if (heldItemText != null)
        {
            heldItemText.gameObject.SetActive(false);
        }

        heldObjRb.isKinematic = false;
        if (heldObjCol) heldObjCol.enabled = true;
        heldObjRb.AddForce(transform.forward * dropForce, ForceMode.Impulse);

        heldObj = null;
        heldObjRb = null;
        heldObjCol = null;
    }

    void FollowHand()
    {
        heldObj.transform.position = Vector3.Lerp(heldObj.transform.position, holdArea.position, followSpeed * Time.deltaTime);
        heldObj.transform.rotation = Quaternion.Lerp(heldObj.transform.rotation, holdArea.rotation, followSpeed * Time.deltaTime);
    }
}