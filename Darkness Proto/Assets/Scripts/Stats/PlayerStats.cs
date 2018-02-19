using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats {

    public PlayerClass playerClass;

	void Start () {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
	}

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            physicalArmor.AddModifier(newItem.physicalArmorModifier);
            magicalArmor.AddModifier(newItem.magicalArmorModifier);
            dodge.AddModifier(newItem.dodgeModifier);
            maxHealth.AddModifier(newItem.healthModifier);
            accuracy.AddModifier(newItem.accuracy);
            critChance.AddModifier(newItem.critChanceModifier);
            critDamage.AddModifier(newItem.critDamageModifier);
            minDamage.AddModifier(newItem.minDamageModifier);
            maxDamage.AddModifier(newItem.maxDamageModifier);
            currentHealth += newItem.healthModifier;
            if(newItem.equipSlot == EquipmentSlot.Weapon)
            {
                damageType = newItem.damageType;
            }
        }
        if (oldItem != null)
        {
            physicalArmor.RemoveModifier(oldItem.physicalArmorModifier);
            magicalArmor.RemoveModifier(oldItem.magicalArmorModifier);
            dodge.RemoveModifier(oldItem.dodgeModifier);
            maxHealth.RemoveModifier(oldItem.healthModifier);
            accuracy.RemoveModifier(newItem.accuracy);
            critChance.RemoveModifier(oldItem.critChanceModifier);
            critDamage.RemoveModifier(oldItem.critDamageModifier);
            minDamage.RemoveModifier(oldItem.minDamageModifier);
            maxDamage.RemoveModifier(oldItem.minDamageModifier);
            currentHealth -= oldItem.healthModifier;
            if (newItem.equipSlot == EquipmentSlot.Weapon)
            {
                damageType = DamageType.Physical;
            }
        }
    }
    public override void Die()
    {
        base.Die();
        PlayerManager.instance.KillPlayer();
    }
}

public enum PlayerClass { Warrior, Rogue, Mage, Paladin }
