using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats {

    public PlayerClass playerClass;
    public float battlePoints;
    private void Start()
    {
        healthBar.transform.Find("Name").GetComponent<Text>().text = name;
    }

    public void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
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
            physicalArmor.AddPercentageModifier(newItem.physicalArmorPercentageModifier);
            magicalArmor.AddPercentageModifier(newItem.magicalArmorPercentageModifier);
            dodge.AddPercentageModifier(newItem.dodgePercentageModifier);
            maxHealth.AddPercentageModifier(newItem.healthPercentageModifier);
            critChance.AddPercentageModifier(newItem.critChancePercentageModifier);
            critDamage.AddPercentageModifier(newItem.critDamagePercentageModifier);
            minDamage.AddPercentageModifier(newItem.minDamagePercentageModifier);
            maxDamage.AddPercentageModifier(newItem.maxDamagePercentageModifier);
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
            physicalArmor.RemovePercentageModifier(oldItem.physicalArmorPercentageModifier);
            magicalArmor.RemovePercentageModifier(oldItem.magicalArmorPercentageModifier);
            dodge.RemovePercentageModifier(oldItem.dodgePercentageModifier);
            maxHealth.RemovePercentageModifier(oldItem.healthPercentageModifier);
            critChance.RemovePercentageModifier(oldItem.critChancePercentageModifier);
            critDamage.RemovePercentageModifier(oldItem.critDamagePercentageModifier);
            minDamage.RemovePercentageModifier(oldItem.minDamagePercentageModifier);
            maxDamage.RemovePercentageModifier(oldItem.minDamagePercentageModifier);
            currentHealth -= oldItem.healthModifier;
            if (newItem.equipSlot == EquipmentSlot.Weapon)
            {
                damageType = DamageType.Physical;
            }
        }
    }
}

public enum PlayerClass { Warrior, Rogue, Mage, Paladin }
