using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "Battle/Monster")]
public class MonsterType : ScriptableObject {

    public string monsterName;

    public int accuracy;
    public float critChance;
    public float critDamage;
    public int minDamage;
    public int maxDamage;
    public DamageType damageType;
    public int maxHealth;
    public float physicalArmor;
    public float magicalArmor;
    public float dodge;
    public int speed;
    public float bleedRes;
    public float blightRes;
    public float fireRes;
    public float stunRes;

    public Skill skill1;
    public Skill skill2;
    public Skill skill3;
}
