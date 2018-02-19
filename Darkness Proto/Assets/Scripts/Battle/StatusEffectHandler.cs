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
        StatusEffect statusEffect = ScriptableObject.CreateInstance<StatusEffect>();
        statusEffect.name = statusEffectType.ToString() + ", " + damage + " (" + duration + ")";
        statusEffect.statusEffectType = statusEffectType;
        statusEffect.damage = damage;
        statusEffect.duration = duration;
        statusEffect.remainingDuration = duration;
        statusEffects.Add(statusEffect);
    }
    public void AddStun(int duration)
    {
        StatusEffect statusEffect = ScriptableObject.CreateInstance<StatusEffect>();
        statusEffect.name =  "Stun, (" + duration + ")";
        statusEffect.statusEffectType = StatusEffectType.Stun;
        statusEffect.duration = duration;
        statusEffect.remainingDuration = duration;
        statusEffects.Add(statusEffect);
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
