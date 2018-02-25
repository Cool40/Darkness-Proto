using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public Stat accuracy;
    public Stat minDamage;
    public Stat maxDamage;
    public Stat physicalArmor;
    public Stat magicalArmor;
    public Stat dodge;
    public Stat critChance;
    public Stat critDamage;
    public Stat maxHealth;
    public Stat speed;
    public Stat maxResource;
    public DamageType damageType;
    public int currentHealth;
    public int currentResource;
    public int currentPosition;

    void Awake()
    {
        currentHealth = (int)maxHealth.GetValue();
        currentResource = (int)maxResource.GetValue();
    }
    public void TakeDamage(int damage, DamageType damageType, string damageSource)
    {
            if(damageType == DamageType.Physical)
            {
                damage = (int)(damage * (1 - physicalArmor.GetValue()));
            }
            else if (damageType == DamageType.Magical)
            {
                damage = (int)(damage * (1 - magicalArmor.GetValue()));
            }
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
            currentHealth -= damage;
            Debug.Log(transform.name + " takes " + damage + " damage from " + damageSource + ".");
            if (currentHealth <= 0)
            {
                Die();
            }
    }
    public virtual void Die()
    {
        // Die in some way
        Debug.Log(transform.name + " died.");
    }
}
