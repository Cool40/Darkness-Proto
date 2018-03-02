using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Stat resourceRegen;
    public DamageType damageType;
    public int currentHealth;
    public int currentResource;
    public int currentPosition;
    public bool isDead;
    public bool hasMoved;

    public Slider healthBar;

    void Awake()
    {
        currentHealth = (int)maxHealth.GetValue();
        currentResource = (int)maxResource.GetValue();
        healthBar.value = 1;
        healthBar.transform.Find("Text").GetComponent<Text>().text = currentHealth + "/" + maxHealth.GetValue();
    }
    public void TakeDamage(int damage, DamageType damageType, string damageSource, string name)
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
            GameObject.Find("Content").GetComponent<Text>().text += name + " takes " + damage + " damage from " + damageSource + ".\n\n";
            if (currentHealth <= 0)
            {
                isDead = true;
            }
        healthBar.value = CalculateHealthPercent();
        healthBar.transform.Find("Text").GetComponent<Text>().text = currentHealth + "/" + maxHealth.GetValue();
    }
    public void Heal(float healPercentageMin, float healPercentageMax, GameObject healSource)
    {
        int damageHealed = (int)(maxHealth.GetValue() * Random.Range(healPercentageMin, healPercentageMax));
        damageHealed = Mathf.Clamp(damageHealed, 0, int.MaxValue);
        Debug.Log(damageHealed);
        healSource.GetComponent<PlayerStats>().battlePoints += (int)(damageHealed * 4);
        currentHealth += damageHealed;
        currentHealth = Mathf.Clamp(currentHealth, 0, (int)maxHealth.GetValue());
        GameObject.Find("Content").GetComponent<Text>().text += name + " regains " + damageHealed + " health from " + healSource.name + ".\n\n";
        healthBar.value = CalculateHealthPercent();
        healthBar.transform.Find("Text").GetComponent<Text>().text = currentHealth + "/" + maxHealth.GetValue();
    }
    float CalculateHealthPercent()
    {
        return currentHealth / maxHealth.GetValue();
    }
    public virtual void Die(string name)
    {
        currentHealth = 0;
        Destroy(gameObject);
        Destroy(healthBar.gameObject);
    }
}
