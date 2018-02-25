using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Skill", menuName = "Battle/Skill")]
public class Skill : ScriptableObject {

    new public string name = "New Skill";
    [Space(10)]
    public ClassRequired classRequired;
    public bool isActivated = false;
    [Space(10)]
    public int resourceCost = 0;
    public int accuracy = 0;
    public int skillCooldown = 0;

    [Header("Target Properties")]

    public SkillTarget[] skillTargets;
    [Range(1, 4)]
    public int[] playerPositions;
    [Range(1, 4)]
    public int[] enemyPositions;
    [Range(0, 4)]
    public int enemyPositionsCount;
    [Range(0, 2)]
    public int minSpaceBetweenEnemies;
    [Range(0, 2)]
    public int maxSpaceBetweenEnemies;
    [Range(1, 4)]
    public int[] allyPositions;
    [Range(0, 4)]
    public int allyPositionsCount;
    [Range(0, 2)]
    public int minSpaceBetweenAllies;
    [Range(0, 2)]
    public int maxSpaceBetweenAllies;

    [Header("Damage Properties")]

    public bool isDamagingSkill = false;
    public bool alwaysCrits = false;
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

    [Header("Critical Regen Buff Properties")]

    public bool isCritRegenBuffSkill = false;
    public float critRegenPercent = 0f;
    public int critRegenDuration;

    [Header("Mana Regen Buff Properties")]

    public bool isManaRegenBuffSkill = false;
    public float manaRegenPercent = 0f;
    public int manaRegenDuration;

    [Header("Damage Reduction Buff Properties (Until Attacked)")]

    public bool isOneTimeDamageReductionSkill = false;
    public float oneTimeDamageReductionPercent = 0f;

    [Header("Accuracy Debuff Properties")]

    public bool isAccuracyDebuffSkill = false;
    public float accuracyDebuffPercent = 0f;
    public int accuracyDebuffDuration = 0;

    [Header("Stun Res Debuff Properties (Flat)")]

    public bool isStunResDebuffSkill = false;
    public float stunResDebuffPercent = 0f;
    public int stunResDebuffDuration = 0;

    [Header("Displacement Properties")]

    public bool isDisplacementSkill = false;
    [Range(0, 3)]
    public int forwardDisplacement = 0;
    [Range(0, 3)]
    public int backwardDisplacement = 0;

    [Header("Self-Displacement Properties")]

    public bool isSelfDisplacementSkill = false;
    [Range(0, 3)]
    public int forwardSelfDisplacement = 0;
    [Range(0, 3)]
    public int backwardSelfDisplacement = 0;

    public void UseSkillOnEnemy(Enemy enemy, PlayerStats playerStats, GameObject battleLog)
    {
        battleLog.GetComponent<Text>().text += playerStats.name + " uses " + name + " on " + enemy.monsterSpecies.monsterName + "\n\n";
        float accuracyAverage = (accuracy + playerStats.accuracy.GetValue()) / 200;
        bool hasHit = Random.value < (accuracyAverage - enemy.GetComponent<EnemyStats>().dodge.GetValue());
        if (hasHit)
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
        else
        {
            Debug.Log(enemy.monsterSpecies.monsterName + " dodges the attack!");
        }
    }
    public void UseSkillOnEnemyMonster(Enemy enemy, PlayerStats playerStats)
    {
        Debug.Log(enemy.monsterSpecies.monsterName + " uses " + name);
        float accuracyAverage = (accuracy + enemy.GetComponent<EnemyStats>().accuracy.GetValue()) / 200;
        bool hasHit = Random.value < (accuracyAverage - playerStats.dodge.GetValue());
        if (hasHit)
        {
            enemy.GetComponent<EnemyStats>().currentResource -= resourceCost;
            if (isDamagingSkill)
            {
                int damage = 0;
                damage = CalculateDamage(enemy.GetComponent<EnemyStats>());
                if (enemy.GetComponent<EnemyStats>().critChance.GetValue() >= Random.value)
                {
                    Debug.Log("Crit!");
                    damage = (int)(damage * enemy.GetComponent<EnemyStats>().critDamage.GetValue());
                }
                Debug.Log(enemy.monsterSpecies.monsterName + " deals " + damage + " damage.");
                playerStats.GetComponent<CharacterStats>().TakeDamage(damage, enemy.GetComponent<EnemyStats>().damageType, name);
            }
            if (isBleedingSkill)
            {
                if (Random.value < bleedChance)
                {
                    int newBleedDamage = (int)(bleedDamage * (damagePercent / 100 * (Random.Range(enemy.GetComponent<EnemyStats>().minDamage.GetValue(), enemy.GetComponent<EnemyStats>().maxDamage.GetValue()))));
                    Debug.Log(enemy.monsterSpecies.monsterName + " causes " + playerStats.name + " to bleed for " + newBleedDamage + " for the next " + bleedDuration + " turns.");
                    playerStats.GetComponent<StatusEffectHandler>().AddStatusEffect(StatusEffectType.Bleed, newBleedDamage, bleedDuration);
                }
            }
            if (isBlightingSkill)
            {
                if (Random.value < blightChance)
                {
                    int newBlightDamage = (int)(blightDamage * (damagePercent / 100 * (Random.Range(enemy.GetComponent<EnemyStats>().minDamage.GetValue(), enemy.GetComponent<EnemyStats>().maxDamage.GetValue()))));
                    Debug.Log(enemy.monsterSpecies.monsterName + " causes " + playerStats.name + " to blight for " + newBlightDamage + " for the next " + blightDuration + " turns.");
                    playerStats.GetComponent<StatusEffectHandler>().AddStatusEffect(StatusEffectType.Blight, newBlightDamage, blightDuration);
                }
            }
            if (isBurningSkill)
            {
                if (Random.value < burnChance)
                {
                    int newBurnDamage = (int)(burnDamage * (damagePercent / 100 * (Random.Range(enemy.GetComponent<EnemyStats>().minDamage.GetValue(), enemy.GetComponent<EnemyStats>().maxDamage.GetValue()))));
                    Debug.Log(enemy.monsterSpecies.monsterName + " causes " + playerStats.name + " to bleed for " + newBurnDamage + " for the next " + burnDuration + " turns.");
                    playerStats.GetComponent<StatusEffectHandler>().AddStatusEffect(StatusEffectType.Burn, newBurnDamage, burnDuration);
                }
            }
            if (isStunningSkill)
            {
                if (Random.value < stunChance)
                {
                    Debug.Log(enemy.monsterSpecies.monsterName + " causes " + playerStats.name + " to be stunned for the next " + stunDuration + " turns.");
                    playerStats.GetComponent<StatusEffectHandler>().AddStun(stunDuration);
                }
            }
        }
        else
        {
            Debug.Log(playerStats.name + " dodges the attack!");
        }
    }
    public void UseSkillOnAlly(PlayerStats allyStats, PlayerStats playerStats)
    {
        if (isDamageBuffSkill)
        {
            Debug.Log(allyStats.name + " gains " + damageBuffPercent + "% damage for the next " + damageBuffDuration + " turns.");
            allyStats.GetComponent<BuffHandler>().AddBuff(damageBuffDuration);
            allyStats.minDamage.AddPercentageModifier(damageBuffPercent / 100);
            allyStats.maxDamage.AddPercentageModifier(damageBuffPercent / 100);
        }
    }
    public void UseSkillOnSelf(PlayerStats playerStats)
    {
        if (isDamageBuffSkill)
        {
            Debug.Log("You gain " + +damageBuffPercent + "% damage for the next " + damageBuffDuration + " turns.");
            playerStats.GetComponent<BuffHandler>().AddBuff(damageBuffDuration);
            playerStats.minDamage.AddPercentageModifier(damageBuffPercent / 100);
            playerStats.maxDamage.AddPercentageModifier(damageBuffPercent / 100);
        }
    }
    public void UseSkillOnSelfMonster(Enemy enemy)
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
    public int CalculateDamage(EnemyStats enemyStats)
    {
        if (damageValueType == DamageValueType.Minimal)
        {
            return (int)(damagePercent / 100 * enemyStats.minDamage.GetValue());
        }
        else if (damageValueType == DamageValueType.Random)
        {
            return (int)(damagePercent / 100 * (Random.Range(enemyStats.minDamage.GetValue(), enemyStats.maxDamage.GetValue())));
        }
        else
        {
            return (int)(damagePercent / 100 * enemyStats.maxDamage.GetValue());
        }
    }

}

public enum SkillTarget { Enemy, Self, Ally }
public enum ClassRequired { Warrior, Rogue, Mage, Paladin, Other }
public enum DamageValueType { Minimal, Random, Maximal}