using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTooltip : MonoBehaviour {

    public GameObject statsPanel;

    private void OnMouseEnter()
    {
        transform.parent.GetComponent<Enemy>().isMouseOverEnemy = true;
        statsPanel.SetActive(true);
        EnemyStats stats = GetComponentInParent<EnemyStats>();
        statsPanel.transform.Find("Stats Text").GetComponent<Text>().text =
            GetComponentInParent<Enemy>().monsterSpecies.monsterName + ", lvl " + GetComponentInParent<Enemy>().monsterLevel + "\n\n" +
            "Health: " + stats.currentHealth + "/" + stats.maxHealth.GetValue() + "\n" +
            "Mana: " + stats.currentResource + "/" + stats.maxResource.GetValue() + "\n" +
            "Accuracy: " + GetComponentInParent<Enemy>().monsterSpecies.accuracy + "\n" +
            "Damage: " + stats.minDamage.GetValue() + "-" + stats.maxDamage.GetValue() + " (" + stats.damageType + ")\n" +
            "Physical Armor: " + stats.physicalArmor.GetValue() + "\n" +
            "Magical Armor: " + stats.magicalArmor.GetValue() + "\n" +
            "Dodge: " + stats.dodge.GetValue() * 100 + "%\n" +
            "Crit Chance: " + stats.critChance.GetValue() * 100 + "%\n" +
            "Crit Damage: " + stats.critDamage.GetValue() + "x\n" +
            "Speed: " + GetComponentInParent<Enemy>().monsterSpecies.speed;
        //show status effect ui for that object
        List<StatusEffect> statusEffects = GetComponentInParent<StatusEffectHandler>().statusEffects;
        foreach (StatusEffect statusEffect in statusEffects)
        {
            statsPanel.transform.Find("Status Effects Text").GetComponent<Text>().text +=
                statusEffect.statusEffectType + ", " + statusEffect.damage + " dmg (" + statusEffect.remainingDuration + ")";
        }
    }
    private void OnMouseExit()
    {
        transform.parent.GetComponent<Enemy>().isMouseOverEnemy = false;
        statsPanel.SetActive(false);
        statsPanel.transform.Find("Stats Text").GetComponent<Text>().text = "";
        statsPanel.transform.Find("Status Effects Text").GetComponent<Text>().text = "";
        //hide status effect ui
    }
}
