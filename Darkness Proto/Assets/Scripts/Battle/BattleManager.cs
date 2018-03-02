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

    public List<GameObject> player;
    public Image[] skillsUI;
    public GameObject battleUI;
    public GameObject battleLog;
    public List<Enemy> enemies;
    public GameObject[] enemyGraphics;
    public GameObject warriorGraphics;
    public GameObject rogueGraphics;
    public GameObject mageGraphics;
    public GameObject paladinGraphics;
    public Camera battleCam;
    public Camera roomCam;
    int goldReward;
    int experienceReward;
    public bool[] chosenEnemies;
    public bool[] chosenAllies;
    bool isPointerOverEnemy;
    public List<int> chosenEnemyPositions;
    public List<int> chosenAllyPositions;
    public GameObject[] enemyToggles;
    public GameObject[] allyToggles;
    Skill currentUsedSkill;
    int turnCounter;
    public float[] speeds;
    GameObject movedObject;
    int usedSkillIndex;

    private void Start()
    {
        enemyGraphics = new GameObject[4];
    }

    private void Update()
    {
       /* foreach (Enemy enemy in enemies)
        {
            if (Input.GetMouseButtonDown(0) && enemy.isMouseOverEnemy)
            {
                AddEnemyPosition(enemyToggles[enemy.GetComponent<EnemyStats>().currentPosition - 1]);
            }
        } */
    }

    public void StartBattle(Camera battleCam, Camera roomCam, List<Enemy> enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            goldReward += (int)(enemy.monsterSpecies.killReward * Mathf.Pow(enemy.monsterSpecies.goldPerLevelMultiplier, enemy.monsterLevel));
        }
        foreach (Enemy enemy in enemies)
        {
            experienceReward += Mathf.CeilToInt(Mathf.Pow(enemy.monsterLevel, 1.1f) + 1);
        }
        foreach (GameObject player in player)
        {
            player.GetComponent<PlayerStats>().battlePoints = 0;
        }
        this.enemies = enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyGraphics[i] = enemies[i].transform.GetChild(0).gameObject;
            enemyGraphics[i].SetActive(true);
        }
        foreach (Enemy enemy in enemies)
        {
            enemy.GetComponent<EnemyStats>().healthBar.transform.Find("Text").GetComponent<Text>().text = enemy.GetComponent<EnemyStats>().currentHealth + "/" + enemy.GetComponent<EnemyStats>().maxHealth.GetValue();
        }
        warriorGraphics = battleCam.transform.parent.Find("Warrior GFX").gameObject;
        warriorGraphics.SetActive(true);
        rogueGraphics = battleCam.transform.parent.Find("Rogue GFX").gameObject;
        rogueGraphics.SetActive(true);
        mageGraphics = battleCam.transform.parent.Find("Mage GFX").gameObject;
        mageGraphics.SetActive(true);
        paladinGraphics = battleCam.transform.parent.Find("Paladin GFX").gameObject;
        paladinGraphics.SetActive(true);
        player[0].transform.parent.Find("GFX").gameObject.SetActive(false);
        chosenEnemies = new bool[enemies.Count];
        chosenAllies = new bool[player.Count];
        speeds = new float[player.Count + enemies.Count];
        battleUI.SetActive(true);
        this.roomCam = roomCam;
        this.roomCam.enabled = false;
        this.battleCam = battleCam;
        this.battleCam.enabled = true;
        int foreachIndex = 0;
        CheckForSpeed();
        foreach (GameObject player in player)
        {
            if (player.GetComponent<PlayerStats>().speed.GetValue() == speeds[0] && !player.GetComponent<PlayerStats>().hasMoved)
            {
                movedObject = player;
                player.GetComponent<PlayerStats>().hasMoved = true;
                break;
            }
        }
        if (movedObject == null)
        {
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetComponent<EnemyStats>().speed.GetValue() == speeds[0] && !enemy.GetComponent<EnemyStats>().hasMoved)
                {
                    movedObject = enemy.gameObject;
                    enemy.GetComponent<EnemyStats>().hasMoved = true;
                    break;
                }
            }
        }
        if (movedObject.GetComponent<PlayerSkills>() != null)
        {
            foreach (Skill skill in movedObject.GetComponent<PlayerSkills>().activatedSkills)
            {
                skillsUI[foreachIndex].GetComponentInChildren<Text>().text = skill.name;
                foreachIndex++;
            }
        }
        InitializeTurn();
    }
    public void EndBattle()
    {
        warriorGraphics.SetActive(false);
        rogueGraphics.SetActive(false);
        mageGraphics.SetActive(false);
        paladinGraphics.SetActive(false);
        float battlePointsSum = 0;
        foreach(GameObject player in player)
        {
            battlePointsSum += player.GetComponent<PlayerStats>().battlePoints;
        }
        foreach (GameObject player in player)
        {
            battleCam.GetComponentInParent<BattleRewards>().AddExperience(player.GetComponent<PlayerStats>(), (int)((player.GetComponent<PlayerStats>().battlePoints / battlePointsSum) * experienceReward));
            Debug.Log(player.name + " gains " + (int)((player.GetComponent<PlayerStats>().battlePoints / battlePointsSum) * experienceReward) + " experience out of " + experienceReward + " possible");
        }
        battleCam.GetComponentInParent<BattleRewards>().AddGold(goldReward);
        GameObject.Find("Player").transform.Find("GFX").gameObject.SetActive(true);
        battleUI.SetActive(false);
        roomCam.enabled = true;
        battleCam.enabled = false;
        Destroy(battleCam.transform.parent.gameObject);
    }
    public void InitializeTurn()
    {
        for (int i = 1; i <= player.Count + enemies.Count; i++)
        {
            if (enemies.Count == 0)
            {
                EndBattle();
            }
            if (speeds[0] == 0)
            {
                CheckForSpeed();
            }
            foreach (GameObject player in player)
            {
                if (player.GetComponent<PlayerStats>().speed.GetValue() == speeds[0] && !player.GetComponent<PlayerStats>().hasMoved)
                {
                    movedObject = player;
                    player.GetComponent<PlayerStats>().hasMoved = true;
                    break;
                }
            }
            if (movedObject == null)
            {
                foreach (Enemy enemy in enemies)
                {
                    if (enemy.GetComponent<EnemyStats>().speed.GetValue() == speeds[0] && !enemy.GetComponent<EnemyStats>().hasMoved)
                    {
                        movedObject = enemy.gameObject;
                        enemy.GetComponent<EnemyStats>().hasMoved = true;
                        break;
                    }
                }
            }
            if(movedObject.GetComponent<Enemy>() != null)
            {
                battleLog.GetComponent<Text>().text += "<b>Turn of " + movedObject.GetComponent<Enemy>().monsterSpecies.monsterName + " <" + movedObject.GetComponent<EnemyStats>().currentPosition + "> starts. </b>\n\n";
                HandleEnemyStatusEffects(movedObject.GetComponent<Enemy>());
                if (movedObject.GetComponent<EnemyStats>().isDead)
                {
                    foreach (float speed in speeds)
                    {
                        if (speed == movedObject.GetComponent<EnemyStats>().speed.GetValue())
                        {
                            speeds[Array.IndexOf(speeds, speed)] = 0;
                            break;
                        }
                    }
                    enemies.Remove(movedObject.GetComponent<Enemy>());
                    movedObject.GetComponent<EnemyStats>().Die(movedObject.name);
                }
                movedObject.GetComponent<EnemyStats>().currentResource += (int)movedObject.GetComponent<EnemyStats>().resourceRegen.GetValue();
                if (movedObject.GetComponent<EnemyStats>().currentResource > movedObject.GetComponent<EnemyStats>().maxResource.GetValue())
                {
                    movedObject.GetComponent<EnemyStats>().currentResource = (int)movedObject.GetComponent<EnemyStats>().maxResource.GetValue();
                }
                Mathf.Clamp(movedObject.GetComponent<EnemyStats>().currentResource, 0, movedObject.GetComponent<EnemyStats>().maxResource.GetValue());
                if (IsStunned(movedObject))
                {
                    battleLog.GetComponent<Text>().text += movedObject.GetComponent<Enemy>().monsterSpecies.monsterName + " <" + movedObject.GetComponent<EnemyStats>().currentPosition + "> is stunned!\n\n";
                }
                else
                {
                    movedObject.GetComponent<Enemy>().MoveEnemy(player, battleLog);
                }
                movedObject = null;
                speeds = speeds.ToArray();
                speeds[Array.IndexOf(speeds, speeds.Max<float>())] = 0;
                Array.Sort(speeds, new Comparison<float>((i1, i2) => i2.CompareTo(i1)));
            }
            else
            {
                battleLog.GetComponent<Text>().text += "<b>Turn of " + movedObject.name + " <" + movedObject.GetComponent<PlayerStats>().currentPosition + "> starts. </b>\n\n";
                HandlePlayerStatusEffects(movedObject);
                movedObject.GetComponent<PlayerStats>().currentResource += (int)movedObject.GetComponent<PlayerStats>().resourceRegen.GetValue();
                if (movedObject.GetComponent<PlayerStats>().currentResource > movedObject.GetComponent<PlayerStats>().maxResource.GetValue())
                {
                    movedObject.GetComponent<PlayerStats>().currentResource = (int)movedObject.GetComponent<PlayerStats>().maxResource.GetValue();
                }
                Mathf.Clamp(movedObject.GetComponent<PlayerStats>().currentResource, 0, movedObject.GetComponent<PlayerStats>().maxResource.GetValue());
                speeds[Array.IndexOf(speeds, speeds.Max<float>())] = 0;
                Array.Sort(speeds, new Comparison<float>((i1, i2) => i2.CompareTo(i1)));
                if (IsStunned(movedObject))
                {
                    battleLog.GetComponent<Text>().text += movedObject.name + " <" + movedObject.GetComponent<PlayerStats>().currentPosition + "> is stunned!\n\n";
                }
                else
                {
                    break;
                }
            }
        }
        for (int i = 1; i <= enemyToggles.Length; i++)
        {
            enemyToggles[i-1].SetActive(false);
        }
    }
    public void GetUsedSkill()
    {
        foreach (bool chosenEnemy in chosenEnemies)
        {
            chosenEnemies[Array.IndexOf(chosenEnemies, chosenEnemy)] = false;
        }
        foreach (GameObject toggle in enemyToggles)
        {
            toggle.GetComponent<Toggle>().isOn = false;
            toggle.SetActive(false);
        }
        foreach (bool chosenAlly in chosenAllies)
        {
            chosenAllies[Array.IndexOf(chosenAllies, chosenAlly)] = false;
        }
        foreach (GameObject toggle in allyToggles)
        {
            toggle.GetComponent<Toggle>().isOn = false;
            toggle.SetActive(false);
        }
        battleUI.transform.Find("Selected Skill").GetComponentInChildren<Text>().text = "Current Selection: None";
        usedSkillIndex = Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name.Substring(6, 1));
        Mathf.Clamp(usedSkillIndex, 0, 4);
        currentUsedSkill = movedObject.GetComponent<PlayerSkills>().activatedSkills[usedSkillIndex];
        battleUI.transform.Find("Selected Skill").GetComponentInChildren<Text>().text = "Current Selection: " + currentUsedSkill.name;
        bool containsEnemyPositionOne = false;
        foreach (Enemy enemy in enemies)
        {
            if(enemy.GetComponent<EnemyStats>().currentPosition == 1)
            {
                containsEnemyPositionOne = true;
                break;
            }
        }
        bool containsEnemyPositionTwo = false;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.GetComponent<EnemyStats>().currentPosition == 2)
            {
                containsEnemyPositionTwo = true;
                break;
            }
        }
        bool containsEnemyPositionThree = false;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.GetComponent<EnemyStats>().currentPosition == 3)
            {
                containsEnemyPositionThree = true;
                break;
            }
        }
        bool containsEnemyPositionFour = false;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.GetComponent<EnemyStats>().currentPosition == 4)
            {
                containsEnemyPositionFour = true;
                break;
            }
        }
        if (currentUsedSkill.enemyPositions.Contains(1) && containsEnemyPositionOne)
        {
            enemyToggles[0].SetActive(true);
        }
        if (currentUsedSkill.enemyPositions.Contains(2) && containsEnemyPositionTwo)
        {
            enemyToggles[1].SetActive(true);
        }
        if (currentUsedSkill.enemyPositions.Contains(3) && containsEnemyPositionThree)
        {
            enemyToggles[2].SetActive(true);
        }
        if (currentUsedSkill.enemyPositions.Contains(4) && containsEnemyPositionFour)
        {
            enemyToggles[3].SetActive(true);
        }
        bool containsAllyPositionOne = false;
        foreach (GameObject ally in player)
        {
            if (ally.GetComponent<PlayerStats>().currentPosition == 1)
            {
                containsAllyPositionOne = true;
                break;
            }
        }
        bool containsAllyPositionTwo = false;
        foreach (GameObject ally in player)
        {
            if (ally.GetComponent<PlayerStats>().currentPosition == 2)
            {
                containsAllyPositionTwo = true;
                break;
            }
        }
        bool containsAllyPositionThree = false;
        foreach (GameObject ally in player)
        {
            if (ally.GetComponent<PlayerStats>().currentPosition == 3)
            {
                containsAllyPositionThree = true;
                break;
            }
        }
        bool containsAllyPositionFour = false;
        foreach (GameObject ally in player)
        {
            if (ally.GetComponent<PlayerStats>().currentPosition == 4)
            {
                containsAllyPositionFour = true;
                break;
            }
        }
        if (currentUsedSkill.allyPositions.Contains(1) && containsAllyPositionOne && movedObject.GetComponent<PlayerStats>().currentPosition != 1)
        {
            allyToggles[0].SetActive(true);
        }
        if (currentUsedSkill.allyPositions.Contains(2) && containsAllyPositionTwo)
        {
            allyToggles[1].SetActive(true);
        }
        if (currentUsedSkill.allyPositions.Contains(3) && containsAllyPositionThree)
        {
            allyToggles[2].SetActive(true);
        }
        if (currentUsedSkill.allyPositions.Contains(4) && containsAllyPositionFour)
        {
            allyToggles[3].SetActive(true);
        }
    }
    public void MovePlayer()
    {
        int chosenEnemiesCount = 0;
        foreach (bool chosenEnemy in chosenEnemies)
        {
            if(chosenEnemy)
            {
                chosenEnemiesCount++;
            }
        }
        if (currentUsedSkill.resourceCost <= movedObject.GetComponent<PlayerStats>().currentResource && currentUsedSkill != null && currentUsedSkill.enemyPositionsCount == chosenEnemiesCount)
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
            int foreachIndexII = 0;
            foreach (bool chosenAlly in chosenAllies)
            {
                foreachIndexII++;
                if (chosenAlly)
                {
                    chosenAllyPositions.Add(foreachIndexII);
                }
            }
            if (currentUsedSkill.skillTargets.Contains(SkillTarget.Enemy))
            {
                foreach (int chosenEnemyPosition in chosenEnemyPositions)
                {
                    foreach (Enemy enemy in enemies.ToList())
                    {
                        if (enemy.GetComponent<EnemyStats>().currentPosition == chosenEnemyPosition)
                        {
                            currentUsedSkill.UseSkillOnEnemy(enemy, movedObject.GetComponent<PlayerStats>(), battleLog);
                            if (enemy.GetComponent<EnemyStats>().isDead)
                            {
                                foreach (float speed in speeds)
                                {
                                    if (speed == enemy.GetComponent<EnemyStats>().speed.GetValue())
                                    {
                                        speeds[Array.IndexOf(speeds, speed)] = 0;
                                        Array.Sort(speeds, new Comparison<float>((i1, i2) => i2.CompareTo(i1)));
                                        break;
                                    }
                                }
                                enemies.Remove(enemy);
                                enemy.GetComponent<EnemyStats>().Die(enemy.name);
                            }
                        }
                    }
                }
            }
            if (currentUsedSkill.skillTargets.Contains(SkillTarget.Ally))
            {
                foreach (int chosenAllyPosition in chosenAllyPositions)
                {
                    foreach (GameObject ally in player.ToList())
                    {
                        if (ally.GetComponent<PlayerStats>().currentPosition == chosenAllyPosition)
                        {
                            currentUsedSkill.UseSkillOnAlly(ally.GetComponent<PlayerStats>(), movedObject.GetComponent<PlayerStats>());
                        }
                    }
                }
            }
            if (currentUsedSkill.skillTargets.Contains(SkillTarget.Self))
            {
                currentUsedSkill.UseSkillOnSelf(movedObject.GetComponent<PlayerStats>());
            }
            movedObject = null;
            InitializeTurn();
            int foreachIndexIII = 0;
            foreach (Skill skill in movedObject.GetComponent<PlayerSkills>().activatedSkills)
            {
                skillsUI[foreachIndexIII].GetComponentInChildren<Text>().text = skill.name;
                foreachIndexIII++;
            }
            foreach (int chosenEnemy in chosenEnemyPositions.ToList())
            {
                chosenEnemyPositions.Remove(chosenEnemy);
            }
            foreach (int chosenAlly in chosenAllyPositions.ToList())
            {
                chosenAllyPositions.Remove(chosenAlly);
            }
            currentUsedSkill = null;
            battleUI.transform.Find("Selected Skill").GetComponentInChildren<Text>().text = "Current Selection: None";
            foreach (bool chosenEnemy in chosenEnemies)
            {
                chosenEnemies[Array.IndexOf(chosenEnemies, chosenEnemy)] = false;
            }
            foreach (bool chosenAlly in chosenAllies)
            {
                chosenAllies[Array.IndexOf(chosenAllies, chosenAlly)] = false;
            }
            foreach (GameObject toggle in enemyToggles)
            {
                toggle.GetComponent<Toggle>().isOn = false;
                toggle.SetActive(false);
            }
            foreach (GameObject toggle in allyToggles)
            {
                toggle.GetComponent<Toggle>().isOn = false;
                toggle.SetActive(false);
            }
        }
    }
    public void Wait()
    {
        movedObject = null;
        InitializeTurn();
        int foreachIndexII = 0;
        foreach (Skill skill in movedObject.GetComponent<PlayerSkills>().activatedSkills)
        {
            skillsUI[foreachIndexII].GetComponentInChildren<Text>().text = skill.name;
            foreachIndexII++;
        }
        foreach (int chosenEnemy in chosenEnemyPositions.ToList())
        {
            chosenEnemyPositions.Remove(chosenEnemy);
        }
        foreach (int chosenAlly in chosenAllyPositions.ToList())
        {
            chosenAllyPositions.Remove(chosenAlly);
        }
        currentUsedSkill = null;
        battleUI.transform.Find("Selected Skill").GetComponentInChildren<Text>().text = "Current Selection: None";
        foreach (bool chosenEnemy in chosenEnemies)
        {
            chosenEnemies[Array.IndexOf(chosenEnemies, chosenEnemy)] = false;
        }
        foreach (bool chosenAlly in chosenAllies)
        {
            chosenAllies[Array.IndexOf(chosenAllies, chosenAlly)] = false;
        }
        foreach (GameObject toggle in enemyToggles)
        {
            toggle.GetComponent<Toggle>().isOn = false;
            toggle.SetActive(false);
         }
        foreach (GameObject toggle in allyToggles)
        {
            toggle.GetComponent<Toggle>().isOn = false;
            toggle.SetActive(false);
        }
    }
    void CheckForSpeed()
        {
        foreach (GameObject player in player)
        {
            player.GetComponent<PlayerStats>().hasMoved = false;
        }
        foreach (Enemy enemy in enemies)
        {
            enemy.GetComponent<EnemyStats>().hasMoved = false;
        }
        turnCounter++;
        if(battleLog.GetComponent<Text>().text.Length > 2000)
        {
            battleLog.GetComponent<Text>().text = null;
        }
        battleLog.GetComponent<Text>().text += "<color=#fff082><Turn " + turnCounter + " has started.></color>\n\n\n";
        int i = 0;
            foreach (GameObject player in player)
            {
                speeds[i] = player.GetComponent<PlayerStats>().speed.GetValue();
                i++;
            }
            foreach (Enemy enemy in enemies)
            {
                if(enemy != null)
                {
                    speeds[i] = enemy.GetComponent<EnemyStats>().speed.GetValue();
                }
                i++;
            }
        Array.Sort(speeds, new Comparison<float>((i1, i2) => i2.CompareTo(i1)));
        //return Array.IndexOf(speeds, speeds.Max<float>());
    }

    void HandlePlayerStatusEffects(GameObject player)
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
                player.GetComponent<PlayerStats>().TakeDamage(statusEffect.damage, DamageType.True, statusEffect.statusEffectType.ToString(), player.name);
            }
            player.GetComponent<StatusEffectHandler>().CheckForRemoval(statusEffect);
        }
    }
    void HandleEnemyStatusEffects(Enemy enemy)
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
                enemy.GetComponent<EnemyStats>().TakeDamage(statusEffect.damage, DamageType.True, statusEffect.statusEffectType.ToString(), enemy.monsterSpecies.monsterName);
            }
        enemy.GetComponent<StatusEffectHandler>().CheckForRemoval(statusEffect);
        }
    }
    public void AddEnemyPosition(GameObject toggle)
    {
        int toggleIndex = Convert.ToInt32(toggle.name.Substring(6, 1));
        bool isExistant = false;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.GetComponent<EnemyStats>().currentPosition == toggleIndex + 1)
            {
                isExistant = true;
            }
        }
        if (currentUsedSkill != null && isExistant)
        {
            chosenEnemies[toggleIndex] = toggle.GetComponent<Toggle>().isOn;
            int chosenEnemiesCount = 0;
            foreach (bool chosenEnemy in chosenEnemies)
            {
                if (chosenEnemy)
                {
                    chosenEnemiesCount++;
                }
            }
            switch (toggleIndex)
            {
                case 0:
                    if (enemyToggles[2].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenEnemies <= 0)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    if (enemyToggles[3].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenEnemies <= 1)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    break;
                case 1:
                    if (enemyToggles[3].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenEnemies <= 0)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    break;
                case 2:
                    if (enemyToggles[0].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenEnemies <= 0)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    break;
                case 3:
                    if (enemyToggles[0].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenEnemies <= 1)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    if (enemyToggles[1].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenEnemies <= 0)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    break;

            }
            bool containsPositionOne = false;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetComponent<EnemyStats>().currentPosition == 1)
                {
                    containsPositionOne = true;
                    break;
                }
            }
            bool containsPositionTwo = false;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetComponent<EnemyStats>().currentPosition == 2)
                {
                    containsPositionTwo = true;
                    break;
                }
            }
            bool containsPositionThree = false;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetComponent<EnemyStats>().currentPosition == 3)
                {
                    containsPositionThree = true;
                    break;
                }
            }
            bool containsPositionFour = false;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.GetComponent<EnemyStats>().currentPosition == 4)
                {
                    containsPositionFour = true;
                    break;
                }
            }
            if (chosenEnemiesCount >= currentUsedSkill.enemyPositionsCount)
            {
                foreach (GameObject toggle1 in enemyToggles)
                {
                    if (!toggle1.GetComponent<Toggle>().isOn)
                    {
                        toggle1.GetComponent<Toggle>().isOn = false;
                        toggle1.SetActive(false);
                    }
                }
            }
            else if (chosenEnemiesCount < currentUsedSkill.enemyPositionsCount)
            {
                if (!enemyToggles[0].GetComponent<Toggle>().isOn && currentUsedSkill.enemyPositions.Contains(1) && containsPositionOne)
                {
                    enemyToggles[0].SetActive(true);
                }
                if (!enemyToggles[1].GetComponent<Toggle>().isOn && currentUsedSkill.enemyPositions.Contains(2) && containsPositionTwo)
                {
                    enemyToggles[1].SetActive(true);
                }
                if (!enemyToggles[2].GetComponent<Toggle>().isOn && currentUsedSkill.enemyPositions.Contains(3) && containsPositionThree)
                {
                    enemyToggles[2].SetActive(true);
                }
                if (!enemyToggles[3].GetComponent<Toggle>().isOn && currentUsedSkill.enemyPositions.Contains(4) && containsPositionFour)
                {
                    enemyToggles[3].SetActive(true);
                }
            }
        }
    }
    public void AddAllyPosition(GameObject toggle)
    {
        int toggleIndex = Convert.ToInt32(toggle.name.Substring(5, 1));
        bool isExistant = false;
        foreach (GameObject ally in player)
        {
            if (ally.GetComponent<PlayerStats>().currentPosition == toggleIndex + 1)
            {
                isExistant = true;
            }
        }
        if (currentUsedSkill != null && isExistant)
        {
            chosenAllies[toggleIndex] = toggle.GetComponent<Toggle>().isOn;
            int chosenAlliesCount = 0;
            foreach (bool chosenAlly in chosenAllies)
            {
                if (chosenAlly)
                {
                    chosenAlliesCount++;
                }
            }
            switch (toggleIndex)
            {
                case 0:
                    if (allyToggles[2].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenAllies <= 0)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    if (allyToggles[3].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenAllies <= 1)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    break;
                case 1:
                    if (allyToggles[3].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenAllies <= 0)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    break;
                case 2:
                    if (allyToggles[0].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenAllies <= 0)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    break;
                case 3:
                    if (allyToggles[0].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenAllies <= 1)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    if (allyToggles[1].GetComponent<Toggle>().isOn && currentUsedSkill.maxSpaceBetweenAllies <= 0)
                    {
                        toggle.GetComponent<Toggle>().isOn = false;
                        return;
                    }
                    break;

            }
            bool containsPositionOne = false;
            foreach (GameObject ally in player)
            {
                if (ally.GetComponent<PlayerStats>().currentPosition == 1)
                {
                    containsPositionOne = true;
                    break;
                }
            }
            bool containsPositionTwo = false;
            foreach (GameObject ally in player)
            {
                if (ally.GetComponent<PlayerStats>().currentPosition == 2)
                {
                    containsPositionTwo = true;
                    break;
                }
            }
            bool containsPositionThree = false;
            foreach (GameObject ally in player)
            {
                if (ally.GetComponent<PlayerStats>().currentPosition == 3)
                {
                    containsPositionThree = true;
                    break;
                }
            }
            bool containsPositionFour = false;
            foreach (GameObject ally in player)
            {
                if (ally.GetComponent<PlayerStats>().currentPosition == 4)
                {
                    containsPositionFour = true;
                    break;
                }
            }
            if (chosenAlliesCount >= currentUsedSkill.allyPositionsCount)
            {
                foreach (GameObject toggle1 in allyToggles)
                {
                    if (!toggle1.GetComponent<Toggle>().isOn)
                    {
                        toggle1.GetComponent<Toggle>().isOn = false;
                        toggle1.SetActive(false);
                    }
                }
            }
            else if (chosenAlliesCount < currentUsedSkill.allyPositionsCount)
            {
                if (!allyToggles[0].GetComponent<Toggle>().isOn && currentUsedSkill.allyPositions.Contains(1) && containsPositionOne && 1 != movedObject.GetComponent<PlayerStats>().currentPosition)
                {
                    allyToggles[0].SetActive(true);
                }
                if (!allyToggles[1].GetComponent<Toggle>().isOn && currentUsedSkill.allyPositions.Contains(2) && containsPositionTwo && 2 != movedObject.GetComponent<PlayerStats>().currentPosition)
                {
                    allyToggles[1].SetActive(true);
                }
                if (!allyToggles[2].GetComponent<Toggle>().isOn && currentUsedSkill.allyPositions.Contains(3) && containsPositionThree && 3 != movedObject.GetComponent<PlayerStats>().currentPosition)
                {
                    allyToggles[2].SetActive(true);
                }
                if (!allyToggles[3].GetComponent<Toggle>().isOn && currentUsedSkill.allyPositions.Contains(4) && containsPositionFour && 4 != movedObject.GetComponent<PlayerStats>().currentPosition)
                {
                    allyToggles[3].SetActive(true);
                }
            }
        }
    }
    bool IsStunned(GameObject target)
    {
        foreach (StatusEffect statusEffect in target.GetComponent<StatusEffectHandler>().statusEffects)
        {
            if (statusEffect.statusEffectType == StatusEffectType.Stun)
            {
                return true;
            }
        }
        return false;
    }
}
