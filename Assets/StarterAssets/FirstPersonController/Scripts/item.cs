using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Informace o pøedmìtu")]
    public string itemName = "Neznámý pøedmìt"; // Tady napíšeš jméno (napø. "Hasièák")

    [Header("Stav")]
    public bool isPickedUp = false;

    // Zde mùžeš pozdìji pøidat další vìci, 
    // napø. typ pøedmìtu (zbraò/jídlo)
}