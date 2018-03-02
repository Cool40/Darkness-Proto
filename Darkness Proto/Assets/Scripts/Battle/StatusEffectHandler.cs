using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectHandler : MonoBehaviour {
    [HideInInspector]
    public PlayerStats playerStats;
    [HideInInspector]
    public EnemyStats enemyStats;
    public List<StatusEffect> statusEffects;

    private void Start()
    {
        if(gameObject.name == "Player")
        {
            playerStats = GetComponent<PlayerStats>();
            enemyStats = null;
        }
        else if(gameObject.name == "Enemy")
        {
            enemyStats = GetComponent<EnemyStats>();
            playerStats = null;
        }
    }

    public void AddStatusEffect(StatusEffectType statusEffectType, int damage, int duration)
    {
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.statusEffectType == statusEffectType && statusEffect.damage == damage)
            {
                statusEffect.remainingDuration += duration;
                statusEffect.name = statusEffect.statusEffectType.ToString() + ", " + damage + " (" + statusEffect.remainingDuration + ")";
                return;
            }
        }
        StatusEffect newStatusEffect = ScriptableObject.CreateInstance<StatusEffect>();
        newStatusEffect.name = statusEffectType.ToString() + ", " + damage + " (" + duration + ")";
        newStatusEffect.statusEffectType = statusEffectType;
        newStatusEffect.damage = damage;
        newStatusEffect.duration = duration;
        newStatusEffect.remainingDuration = duration;
        statusEffects.Add(newStatusEffect);
    }
    public void AddStun(int duration)
    {
        foreach (StatusEffect statusEffect in statusEffects)
        {
            if (statusEffect.statusEffectType == StatusEffectType.Stun)
            {
                statusEffect.remainingDuration += duration;
                statusEffect.name = "Stun, (" + statusEffect.remainingDuration + ")";
                return;
            }
        }
        StatusEffect newStatusEffect = ScriptableObject.CreateInstance<StatusEffect>();
        newStatusEffect.name =  "Stun, (" + duration + ")";
        newStatusEffect.statusEffectType = StatusEffectType.Stun;
        newStatusEffect.duration = duration;
        newStatusEffect.remainingDuration = duration;
        statusEffects.Add(newStatusEffect);
    }
    public void CheckForRemoval(StatusEffect statusEffect)
    {
        if (statusEffect.remainingDuration == 0 && statusEffect.statusEffectType != StatusEffectType.Stun)
        {
            statusEffects.Remove(statusEffect);
        }
        else if (statusEffect.remainingDuration == -1 && statusEffect.statusEffectType == StatusEffectType.Stun)
        {
            statusEffects.Remove(statusEffect);
        }
    }
}
