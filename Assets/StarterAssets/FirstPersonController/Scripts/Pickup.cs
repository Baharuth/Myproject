using UnityEngine;
using UnityEngine.InputSystem; 

public class PickupSystem : MonoBehaviour
{
    [Header("Settings")]
    public Transform holdArea;
    
  
    public Key pickupKey = Key.F; 
    
    public float pickupRange = 5.0f;
    public float dropForce = 5.0f; 

    private GameObject heldObj;
    private Rigidbody heldObjRb;
    public LayerMask pickupLayer; 

    void Update()
    {
      
        if (Keyboard.current != null && Keyboard.current[pickupKey].wasPressedThisFrame)
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, pickupRange, pickupLayer))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }
        
        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObj = pickObj;
            heldObjRb = pickObj.GetComponent<Rigidbody>();

            heldObjRb.useGravity = false;
            heldObjRb.linearDamping = 10;
            heldObjRb.constraints = RigidbodyConstraints.FreezeRotation;

            heldObj.transform.parent = holdArea;
            heldObj.transform.localPosition = Vector3.zero;
            heldObj.transform.localRotation = Quaternion.identity;
        }
    }

    void DropObject()
    {
        heldObjRb.useGravity = true;
        heldObjRb.linearDamping = 1;
        heldObjRb.constraints = RigidbodyConstraints.None;

        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * dropForce, ForceMode.Impulse);
        heldObj = null;
    }

    void MoveObject()
    {
        
    }
}