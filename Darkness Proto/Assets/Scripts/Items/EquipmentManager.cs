using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour {

    public Equipment[] defaultItems;
    Equipment[] currentEquipment;
    PlayerLevel playerLevel;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    private void Start()
    {
        inventory = Inventory.instance;
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        playerLevel = GetComponent<PlayerLevel>();
        onEquipmentChanged += GetComponent<PlayerStats>().OnEquipmentChanged;
        EquipDefaultItems();
    }
    public void Equip(Equipment newItem)
    {
        if(newItem.requiredLevel <= playerLevel.currentLevel)
        {
            int slotIndex = (int)newItem.equipSlot;
            Equipment oldItem = Unequip(slotIndex);
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(newItem, oldItem);
            }
            currentEquipment[slotIndex] = newItem;
        }
        else
        {
            Debug.Log("Your level is too low to wear this item.");
        }
    }
    public Equipment Unequip (int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }
    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
        EquipDefaultItems();
    }
    void EquipDefaultItems()
    {
        foreach(Equipment item in defaultItems)
        {
            Equip(item);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }
}
