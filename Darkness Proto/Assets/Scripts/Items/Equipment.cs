using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item {

    EquipmentManager equipmentManager;

    public EquipmentSlot equipSlot;
    public DamageType damageType;
    public ClassRequired classRequired;

    [Range(1, 50)]
    public int requiredLevel;

    [Header("Defensive Flat Modifiers")]
    public float physicalArmorModifier;
    public float magicalArmorModifier;
    public float dodgeModifier;
    public int healthModifier;
    [Header("Offensive Flat Modifiers")]
    public int minDamageModifier;
    public int maxDamageModifier;
    public float critChanceModifier;
    public float critDamageModifier;

    [Header("Defensive Percentage Modifiers")]
    public float physicalArmorPercentageModifier;
    public float magicalArmorPercentageModifier;
    public float dodgePercentageModifier;
    public int healthPercentageModifier;
    [Header("Offensive Percentage Modifiers")]
    public int minDamagePercentageModifier;
    public int maxDamagePercentageModifier;
    public float critChancePercentageModifier;
    public float critDamagePercentageModifier;
    [Space(10)]
    public int accuracy;

    public override void Use()
    {
        base.Use();
        equipmentManager.Equip(this);
        RemoveFromInventory();
    }
}

public enum EquipmentSlot { Weapon, Armor }
public enum DamageType { Physical, Magical, True, None }