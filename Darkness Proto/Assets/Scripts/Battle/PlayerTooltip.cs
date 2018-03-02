using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTooltip : MonoBehaviour
{

    public GameObject statsPanel;
    public GameObject player;

    private void OnMouseEnter()
    {
        statsPanel.SetActive(true);
        PlayerStats stats = player.GetComponent<PlayerStats>();
        statsPanel.transform.Find("Stats Text").GetComponent<Text>().text =
            player.name + ", lvl " + player.GetComponent<PlayerLevel>().currentLevel + "\n\n" +
            "Health: " + stats.currentHealth + "/" + stats.maxHealth.GetValue() + "\n" +
            "Mana: " + stats.currentResource + "/" + stats.maxResource.GetValue() + "\n" +
            "Accuracy: " + stats.accuracy.GetValue() + "\n" +
            "Damage: " + stats.minDamage.GetValue() + "-" + stats.maxDamage.GetValue() + " (" + stats.damageType + ")\n" +
            "Physical Armor: " + Mathf.Round(stats.physicalArmor.GetValue() * 100) / 100 + "\n" +
            "Magical Armor: " + Mathf.Round(stats.magicalArmor.GetValue() * 100) / 100 + "\n" +
            "Dodge: " + Mathf.Round(stats.dodge.GetValue() * 10000) / 100 + "%\n" +
            "Crit Chance: " + Mathf.Round(stats.critChance.GetValue() * 10000) / 100 + "%\n" +
            "Crit Damage: " + stats.critDamage.GetValue() + "x\n" +
            "Speed: " + stats.speed.GetValue();
        //show status effect ui for that object
    }
    private void OnMouseExit()
    {
        statsPanel.SetActive(false);
        statsPanel.transform.Find("Stats Text").GetComponent<Text>().text = "";
        //hide status effect ui
    }
}
