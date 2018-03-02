using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public MonsterType monsterSpecies;
    public PlayerStats target;
    public int monsterLevel;
    public bool isMouseOverEnemy;


    public void MoveEnemy(List<GameObject> player, GameObject battleLog)
    {
        switch(monsterSpecies.monsterName)
        {
            case "Skeleton Swordsman":
                if(GetComponent<EnemyStats>().currentPosition != 4)
                {
                    int chosenPosition = Random.Range(1, 4);
                    foreach (GameObject enemy in player)
                    {
                        if (enemy.GetComponent<PlayerStats>().currentPosition == chosenPosition)
                        {
                            target = enemy.GetComponent<PlayerStats>();
                        }
                    }
                    monsterSpecies.skill1.UseSkillOnEnemyMonster(this, target, battleLog);
                }
                else
                {
                    int chosenPosition = Random.Range(1, 3);
                    foreach (GameObject enemy in player)
                    {
                        if (enemy.GetComponent<PlayerStats>().currentPosition == chosenPosition)
                        {
                            target = enemy.GetComponent<PlayerStats>();
                        }
                    }
                    monsterSpecies.skill2.UseSkillOnEnemyMonster(this, target, battleLog);
                    monsterSpecies.skill2.UseSkillOnSelfMonster(this);
                }
                break;
        }
    }
}
