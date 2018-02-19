using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats {

    public override void Die()
    {
        base.Die();

        // Add ragdoll effect/death anim

        Destroy(gameObject);
    }
    void Start()
    {
        accuracy.AddModifier(GetComponent<Enemy>().monsterSpecies.accuracy);
        minDamage.AddModifier(GetComponent<Enemy>().monsterSpecies.minDamage);
        maxDamage.AddModifier(GetComponent<Enemy>().monsterSpecies.maxDamage);
        physicalArmor.AddModifier(GetComponent<Enemy>().monsterSpecies.physicalArmor);
        magicalArmor.AddModifier(GetComponent<Enemy>().monsterSpecies.magicalArmor);
        dodge.AddModifier(GetComponent<Enemy>().monsterSpecies.dodge);
        critChance.AddModifier(GetComponent<Enemy>().monsterSpecies.critChance);
        critDamage.AddModifier(GetComponent<Enemy>().monsterSpecies.critDamage);
        maxHealth.AddModifier(GetComponent<Enemy>().monsterSpecies.maxHealth);
        speed.AddModifier(GetComponent<Enemy>().monsterSpecies.speed);
        damageType = GetComponent<Enemy>().monsterSpecies.damageType;
        currentHealth += GetComponent<Enemy>().monsterSpecies.maxHealth;
    }

}
