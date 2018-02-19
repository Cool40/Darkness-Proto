using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Battle/Skill")]
public class Skill : ScriptableObject {

    new public string name = "New Skill";
    [Space(10)]
    public ClassRequired classRequired;
    public bool isActivated = false;
    [Space(10)]
    public int resourceCost = 0;
    public int accuracy = 0;

    [Header("Target Properties")]

    public SkillTarget[] skillTargets;
    [Range(1, 4)]
    public int[] playerPositions;
    [Range(1, 4)]
    public int[] enemyPositions;
    [Range(1, 4)]
    public int[] allyPositions;

    [Header("Damage Properties")]

    public bool isDamagingSkill = false;
    public float damagePercent = 0f;
    public DamageValueType damageValueType;

    [Header("Bleed Properties")]

    public bool isBleedingSkill = false;
    public float bleedChance = 0f;
    public float bleedDamage = 0f;
    public int bleedDuration = 0;

    [Header("Blight Properties")]

    public bool isBlightingSkill = false;
    public float blightChance = 0f;
    public float blightDamage = 0f;
    public int blightDuration = 0;

    [Header("Burn Properties")]

    public bool isBurningSkill = false;
    public float burnChance = 0f;
    public float burnDamage = 0f;
    public int burnDuration = 0;

    [Header("Stun Properties")]

    public bool isStunningSkill = false;
    public float stunChance = 0f;
    public int stunDuration = 0;

    [Header("Damage Buff Properties")]

    public bool isDamageBuffSkill = false;
    public float damageBuffPercent = 0f;
    public int damageBuffDuration = 0;

    [Header("Critical Regen Properties")]

    public bool isCritRegenBuffSkill = false;
    public float critRegenPercent = 0f;
    public int critRegenDuration;

    [Header("Displacement Properties")]

    public bool isDisplacementSkill = false;
    [Range(0, 3)]
    public int forwardDisplacement = 0;
    [Range(0, 3)]
    public int backwardDisplacement = 0;

    public void UseSkillOnEnemy(Enemy enemy, PlayerStats playerStats)
    {
        Debug.Log("Using " + name);
        float accuracyAverage = (accuracy + playerStats.accuracy.GetValue()) / 200;
        bool hasHit = Random.value < (accuracyAverage - enemy.GetComponent<EnemyStats>().dodge.GetValue());
        if (hasHit && playerStats.currentResource >= resourceCost)
        {
            playerStats.currentResource -= resourceCost;
            if (isDamagingSkill)
            {
                int damage = 0;
                damage = CalculateDamage(playerStats);
                if (playerStats.critChance.GetValue() >= Random.value)
                {
                    Debug.Log("Crit!");
                    damage = (int)(damage * playerStats.critDamage.GetValue());
                }
                Debug.Log("You deal " + damage + " damage.");
                enemy.GetComponent<CharacterStats>().TakeDamage(damage, playerStats.damageType, name);
            }
            if (isBleedingSkill)
            {
                if (Random.value < bleedChance)
                {
                    int newBleedDamage = (int)(bleedDamage *(damagePercent/100 * (Random.Range(playerStats.minDamage.GetValue(), playerStats.maxDamage.GetValue()))));
                    Debug.Log("You cause the enemy to bleed for " + newBleedDamage + " for the next " + bleedDuration + " turns.");
                    enemy.GetComponent<StatusEffectHandler>().AddStatusEffect(StatusEffectType.Bleed, newBleedDamage, bleedDuration);
                }
            }
            if (isBlightingSkill)
            {
                if (Random.value < blightChance)
                {
                    int newBlightDamage = (int)(blightDamage * (damagePercent / 100 * (Random.Range(playerStats.minDamage.GetValue(), playerStats.maxDamage.GetValue()))));
                    Debug.Log("You cause the enemy to blight for " + newBlightDamage + " for the next " + blightDuration + " turns.");
                    enemy.GetComponent<StatusEffectHandler>().AddStatusEffect(StatusEffectType.Blight, newBlightDamage, blightDuration);
                }
            }
            if (isBurningSkill)
            {
                if (Random.value < burnChance)
                {
                    int newBurnDamage = (int)(burnDamage * (damagePercent / 100 * (Random.Range(playerStats.minDamage.GetValue(), playerStats.maxDamage.GetValue()))));
                    Debug.Log("You cause the enemy to burn for " + newBurnDamage + " for the next " + burnDuration + " turns.");
                    enemy.GetComponent<StatusEffectHandler>().AddStatusEffect(StatusEffectType.Burn, newBurnDamage, burnDuration);
                }
            }
            if (isStunningSkill)
            {
                if (Random.value < stunChance)
                {
                    Debug.Log("You cause the enemy to be stunned for the next " + stunDuration + " turns.");
                    enemy.GetComponent<StatusEffectHandler>().AddStun(stunDuration);
                }
            }
        }
        else if (!hasHit && playerStats.currentResource >= resourceCost)
        {
            Debug.Log(enemy.monsterSpecies.monsterName + " dodges the attack!");
        }
        else if (playerStats.currentResource < resourceCost)
        {
            Debug.Log("You lack mana to use that skill!");
            // loop until player chooses cheaper skill
        }
    }
    public void UseSkillOnAlly(PlayerStats playerStats)
    {

    }
    public void UseSkillOnSelf(PlayerStats playerStats)
    {

    }
    public int CalculateDamage(PlayerStats playerStats)
    {
        if (damageValueType == DamageValueType.Minimal)
        {
            return (int)(damagePercent / 100 * playerStats.minDamage.GetValue());
        }
        else if (damageValueType == DamageValueType.Random)
        {
            return (int)(damagePercent / 100 * (Random.Range(playerStats.minDamage.GetValue(), playerStats.maxDamage.GetValue())));
        }
        else
        {
            return (int)(damagePercent / 100 * playerStats.maxDamage.GetValue());
        }
    }

}

public enum SkillTarget { Enemy, Self, Ally }
public enum ClassRequired { Warrior, Rogue, Mage, Paladin, Other }
public enum DamageValueType { Minimal, Random, Maximal}