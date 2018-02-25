using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffHandler : MonoBehaviour {
    [HideInInspector]
    public PlayerStats playerStats;
    [HideInInspector]
    public EnemyStats enemyStats;
    public List<Buff> buffs;

    private void Start()
    {
        if (gameObject.name == "Player")
        {
            playerStats = GetComponent<PlayerStats>();
            enemyStats = null;
        }
        else if (gameObject.name == "Enemy")
        {
            enemyStats = GetComponent<EnemyStats>();
            playerStats = null;
        }
    }
    public void AddBuff(int duration)
    {
        Buff buff = ScriptableObject.CreateInstance<Buff>();
        buff.duration = duration;
        buff.remainingDuration = duration;
        buffs.Add(buff);
    }
    public void CheckForRemoval(Buff buff)
    {
        if (buff.remainingDuration == 0)
        {
            buffs.Remove(buff);
        }
    }
}
