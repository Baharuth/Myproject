using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
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

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[pickupKey].wasPressedThisFrame)
        {
            if (heldObj == null)
            {
                TryPickup();
            }
            else
            {
                DropObject();
            }
        }

        if (heldObj != null)
        {
            FollowHand();
        }
    }

    void TryPickup()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, pickupLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (!rb) return;

            heldObj = rb.gameObject;
            heldObjRb = rb;
            heldObjCol = heldObj.GetComponent<Collider>();

            // Pokusíme se najít skript Item kdekoli na objektu
            Item item = heldObj.GetComponent<Item>();
            if (item == null) item = heldObj.GetComponentInParent<Item>();
            if (item == null) item = heldObj.GetComponentInChildren<Item>();

            if (item != null)
            {
                item.isPickedUp = true;
                Debug.Log("isPickedUp = true");
            }
            else
            {
                // Pokud zvedneš věc, co nemá skript Item, vypíše se varování
                Debug.LogWarning("Zvednuto, ale objekt nemá skript 'Item'!");
            }

            heldObjRb.isKinematic = true;
            if (heldObjCol) heldObjCol.enabled = false;
        }
    }

    void DropObject()
    {
        Item item = heldObj.GetComponent<Item>();
        if (item == null) item = heldObj.GetComponentInParent<Item>();
        if (item == null) item = heldObj.GetComponentInChildren<Item>();

        if (item != null)
        {
            item.isPickedUp = false;
            Debug.Log("isPickedUp = false");
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