using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {

    public EquipmentSlot equipSlot;
    public DamageType damageType;

    [Range(0, 50)]
    public int requiredLevel;

    public float physicalArmorModifier;
    public float magicalArmorModifier;
    public float dodgeModifier;
    public int healthModifier;

    public int minDamageModifier;
    public int maxDamageModifier;
    public float critChanceModifier;
    public float critDamageModifier;
    public int accuracy;

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}

public enum EquipmentSlot { Weapon, Armor }
public enum DamageType { Physical, Magical, True, None }