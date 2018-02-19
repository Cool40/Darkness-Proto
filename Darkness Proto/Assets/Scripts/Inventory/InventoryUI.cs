using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public Transform itemsParent;
    public GameObject inventoryUI;
    Inventory inventory;
    InventorySlot[] slots;
    public PlayerController playerController;

	void Start () {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        playerController = PlayerManager.instance.player.GetComponent<PlayerController>();
	}
	
	void Update () {
		if (Input.GetButtonDown("Inventory") && !playerController.isInBattle)
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
	}
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
