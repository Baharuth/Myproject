using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject pickupMessageUI; // Sem přetáhni Text objekt

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

    // Proměnná pro objekt, na který se právě díváme (ale ještě nedržíme)
    private GameObject currentTargetObj;

    void Start()
    {
        if (pickupMessageUI != null) pickupMessageUI.SetActive(false);
    }

    void Update()
    {
        // 1. Pokud už něco držíme, řešíme jen pohyb a položení
        if (heldObj != null)
        {
            FollowHand();

            // Skryjeme UI a vymažeme cíl, protože máme plné ruce
            if (pickupMessageUI != null) pickupMessageUI.SetActive(false);
            currentTargetObj = null;

            // Položení
            if (Keyboard.current != null && Keyboard.current[pickupKey].wasPressedThisFrame)
            {
                DropObject();
            }
        }
        else
        {
            // 2. Pokud nic nedržíme -> Hledáme (Raycast běží každý frame)
            DetectObjectInFront();

            // Zvednutí (použije výsledek z DetectObjectInFront)
            if (Keyboard.current != null && Keyboard.current[pickupKey].wasPressedThisFrame)
            {
                if (currentTargetObj != null)
                {
                    PerformPickup(currentTargetObj);
                }
            }
        }
    }

    // Tato metoda běží pořád a zjišťuje, na co koukáme
    void DetectObjectInFront()
    {
        RaycastHit hit;

        // Raycast vystřelíme TADY a výsledek si uložíme
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, pickupLayer))
        {
            // Zkontrolujeme, zda je to Item
            Item item = hit.collider.GetComponent<Item>();
            if (item == null) item = hit.collider.GetComponentInParent<Item>();
            if (item == null) item = hit.collider.GetComponentInChildren<Item>();

            if (item != null)
            {
                // Našli jsme validní předmět -> Uložíme ho do proměnné
                currentTargetObj = hit.collider.attachedRigidbody ? hit.collider.attachedRigidbody.gameObject : hit.collider.gameObject;

                // Zapneme UI
                if (pickupMessageUI != null) pickupMessageUI.SetActive(true);
                return;
            }
        }

        // Pokud Raycast nic netrefil nebo to nebyl Item:
        currentTargetObj = null;
        if (pickupMessageUI != null) pickupMessageUI.SetActive(false);
    }

    // Tato metoda se zavolá jen když zmáčkneš F a currentTargetObj existuje
    void PerformPickup(GameObject target)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (!rb) return;

        heldObj = target;
        heldObjRb = rb;
        heldObjCol = heldObj.GetComponent<Collider>();

        // Nastavíme logiku Itemu
        Item item = heldObj.GetComponent<Item>();
        if (item == null) item = heldObj.GetComponentInParent<Item>();
        if (item == null) item = heldObj.GetComponentInChildren<Item>();

        if (item != null)
        {
            item.isPickedUp = true;
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