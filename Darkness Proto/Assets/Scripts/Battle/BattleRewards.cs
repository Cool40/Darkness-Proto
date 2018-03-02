using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRewards : MonoBehaviour {

    public Inventory inventory;

    public void AddExperience(PlayerStats playerStats, int amount)
    {
        playerStats.GetComponent<PlayerLevel>().AddExperience(amount);
    }
    public void AddGold(int amount)
    {
        inventory.gold += amount;
    }
}
