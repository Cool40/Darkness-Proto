using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System;

public class BattleManager : MonoBehaviour {

    #region Singleton

    public static BattleManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of BattleManager found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject[] player;
    public Image[] skillsUI;
    public GameObject battleUI;
    public GameObject battleLog;
    public Enemy[] enemies;
    public bool[] chosenEnemies;
    public bool[] chosenAllies;
    public List<int> chosenEnemyPositions;
    public GameObject[] enemyToggles;
    Skill currentUsedSkill;
    public float[] speeds;
    int movedObject;
    public List<PlayerSkills> playerSkills;
    int usedSkillIndex;

    private void Start()
    {
        for (int i = 0; i < player.Count(); i++)
        {
            playerSkills.Capacity++;
            playerSkills.Add(player[i].GetComponent<PlayerSkills>());
        }
    }
    private void Update()
    {
        //if (usedSkillIndex != -1 && enemies.Length != 0)
        //{
        //    InitializeTurn();
        //}
    }

    public void StartBattle(Camera battleCam, Camera roomCam, Enemy[] enemies)
    {
        this.enemies = enemies;
        chosenEnemies = new bool[enemies.Length];
        speeds = new float[player.Length + enemies.Length];
        battleUI.SetActive(true);
        roomCam.enabled = false;
        battleCam.enabled = true;
        //player.GetComponent<PlayerController>().isInBattle = true;
        //player.GetComponent<NavMeshAgent>().isStopped = true;
        //player.transform.position = new Vector3(battleCam.transform.position.x - 3.5f, player.transform.position.y, battleCam.transform.position.z + 10f);
        //player.transform.rotation = new Quaternion(0, 0, 0, 0);
        movedObject = CheckForSpeed();
        int i = 0;
        foreach (Skill skill in player[movedObject].GetComponent<PlayerSkills>().activatedSkills)
        {
            skillsUI[i].GetComponentInChildren<Text>().text = skill.name;
            i++;
        }
        InitializeTurn();
    }
    public void InitializeTurn()
    {
        //Move fastest enemy
        //Add mana to it
        //Handle status effects
        //Check if stunned
        for (int i = 1; i <= player.Length+enemies.Length; i++)
        {
            if (speeds.Length == 0)
            {
                movedObject = CheckForSpeed();
            }
            else
            {
                movedObject = Array.IndexOf(speeds, speeds.Max<float>());
                speeds[Array.IndexOf(speeds, speeds.Max<float>())] = 0;
                Debug.Log(movedObject);
            }
            if (movedObject >= player.Length)
            {
                //move enemy at index movedOject - player.Lenght + 1
                enemies[movedObject - player.Length].MoveEnemy(player);
                speeds = speeds.ToArray();
                speeds[Array.IndexOf(speeds, speeds.Max<float>())] = 0;
            }
            else
            {
                break;
            }
        }
        for (int i = 1; i <= enemyToggles.Length; i++)
        {
            enemyToggles[i-1].SetActive(false);
        }
    }
    public void GetUsedSkill()
    {
        for (int i = 1; i <= enemyToggles.Length; i++)
        {
            enemyToggles[i - 1].SetActive(false);
        }
        battleUI.transform.Find("Selected Skill").GetComponentInChildren<Text>().text = "Current Selection: None";
        usedSkillIndex = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name.Substring(6, 1));
        Mathf.Clamp(usedSkillIndex, 0, 4);
        currentUsedSkill = playerSkills[movedObject].activatedSkills[usedSkillIndex];
        if (movedObject <= player.Length)
        {
            battleUI.transform.Find("Selected Skill").GetComponentInChildren<Text>().text = "Current Selection: " + currentUsedSkill.name;
        }
        if(currentUsedSkill.enemyPositions.Contains(1))
        {
            enemyToggles[0].SetActive(true);
        }
        if (currentUsedSkill.enemyPositions.Contains(2))
        {
            enemyToggles[1].SetActive(true);
        }
        if (currentUsedSkill.enemyPositions.Contains(3))
        {
            enemyToggles[2].SetActive(true);
        }
        if (currentUsedSkill.enemyPositions.Contains(4))
        {
            enemyToggles[3].SetActive(true);
        }
    }
    public void MovePlayer()
    {
        if (currentUsedSkill.resourceCost <= player[movedObject].GetComponent<PlayerStats>().currentResource)
        {
            int foreachIndex = 0;
            foreach (bool chosenEnemy in chosenEnemies)
            {
                foreachIndex++;
                if (chosenEnemy)
                {
                    chosenEnemyPositions.Add(foreachIndex);
                }
            }
            foreach (int chosenEnemyPosition in chosenEnemyPositions)
            {
            }
            if (currentUsedSkill.skillTargets.Contains(SkillTarget.Enemy))
            {
                foreach (int chosenEnemyPosition in chosenEnemyPositions)
                {
                    currentUsedSkill.UseSkillOnEnemy(enemies[chosenEnemyPosition - 1], player[movedObject].GetComponent<PlayerStats>(), battleLog);
                }
            }
            if (currentUsedSkill.skillTargets.Contains(SkillTarget.Ally))
            {
                currentUsedSkill.UseSkillOnAlly(player[0].GetComponent<PlayerStats>(), player[movedObject].GetComponent<PlayerStats>());
            }
            if (currentUsedSkill.skillTargets.Contains(SkillTarget.Self))
            {
                currentUsedSkill.UseSkillOnSelf(player[movedObject].GetComponent<PlayerStats>());
            }
            InitializeTurn();
            int i = 0;
            if (movedObject < player.Length)
            {
                foreach (Skill skill in player[movedObject].GetComponent<PlayerSkills>().activatedSkills)
                {
                    skillsUI[i].GetComponentInChildren<Text>().text = skill.name;
                    i++;
                }
            }
            chosenEnemyPositions.Clear();
        }
    }

    int CheckForSpeed()
    {
        int i = 0;
            foreach (GameObject player in player)
            {
                speeds[i] = player.GetComponent<PlayerStats>().speed.GetValue();
                i++;
            }
            foreach (Enemy enemy in enemies)
            {
                speeds[i] = enemy.GetComponent<EnemyStats>().speed.GetValue();
                i++;
            }
        return Array.IndexOf(speeds, speeds.Max<float>());
    }

    void HandlePlayerStatusEffects()
    {
        foreach (GameObject player in player)
        {
            foreach (StatusEffect statusEffect in player.GetComponent<StatusEffectHandler>().statusEffects.ToList())
            {
                statusEffect.remainingDuration--;
                if (statusEffect.statusEffectType == StatusEffectType.Stun)
                {
                    statusEffect.name = "Stun, (" + statusEffect.remainingDuration + ")";
                }
                else
                {
                    statusEffect.name = statusEffect.statusEffectType.ToString() + ", " + statusEffect.damage + " (" + statusEffect.remainingDuration + ")";
                    player.GetComponent<PlayerStats>().TakeDamage(statusEffect.damage, DamageType.True, statusEffect.statusEffectType.ToString());
                }
                player.GetComponent<StatusEffectHandler>().CheckForRemoval(statusEffect);
            }
        }

    }
    void HandleEnemyStatusEffects()
    {
        foreach (Enemy enemy in enemies)
        {
            foreach (StatusEffect statusEffect in enemy.GetComponent<StatusEffectHandler>().statusEffects.ToList())
            {
                statusEffect.remainingDuration--;
                if (statusEffect.statusEffectType == StatusEffectType.Stun)
                {
                    statusEffect.name = "Stun, (" + statusEffect.remainingDuration + ")";
                }
                else
                {
                    statusEffect.name = statusEffect.statusEffectType.ToString() + ", " + statusEffect.damage + " (" + statusEffect.remainingDuration + ")";
                    enemy.GetComponent<EnemyStats>().TakeDamage(statusEffect.damage, DamageType.True, statusEffect.statusEffectType.ToString());
                }
                enemy.GetComponent<StatusEffectHandler>().CheckForRemoval(statusEffect);
            }

        }
    }
    public void AddEnemyPosition(GameObject toggle)
    {
        chosenEnemies[(Convert.ToInt32(toggle.name.Substring(6, 1)))] = !chosenEnemies[(Convert.ToInt32(toggle.name.Substring(6, 1)))];
        int countOfTrue = 0;
        foreach(GameObject enemyToggle in enemyToggles)
        {
            if (chosenEnemies[(Convert.ToInt32(enemyToggle.name.Substring(6, 1)))])
            {
                countOfTrue++;
            }
        }
        foreach (GameObject enemyToggle in enemyToggles)
        {
            if (enemyToggle != toggle && currentUsedSkill.enemyPositionsCount == countOfTrue && !chosenEnemies[Array.IndexOf(enemyToggles, enemyToggle)])
            {
                enemyToggle.SetActive(false);
            }
        }
        if(!chosenEnemies.Contains(true))
        {
            if (currentUsedSkill.enemyPositions.Contains(1))
            {
                enemyToggles[0].SetActive(true);
            }
            if (currentUsedSkill.enemyPositions.Contains(2))
            {
                enemyToggles[1].SetActive(true);
            }
            if (currentUsedSkill.enemyPositions.Contains(3))
            {
                enemyToggles[2].SetActive(true);
            }
            if (currentUsedSkill.enemyPositions.Contains(4))
            {
                enemyToggles[3].SetActive(true);
            }
        }
    }
    bool IsStunned(GameObject target)
    {
        return false;
    }
}
