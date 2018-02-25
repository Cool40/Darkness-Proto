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
    public DamageType damageType;
    public int currentHealth;
    public int currentResource;
    public int currentPosition;

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
                Die(name);
            }
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
