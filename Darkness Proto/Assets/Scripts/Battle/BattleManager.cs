using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

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

    public GameObject player;
    public Image[] skillsUI;
    public GameObject battleUI;
    public Enemy enemy;
    PlayerSkills playerSkills;
    bool isPlayersTurn = false;
    int usedSkillIndex = -1;

    private void Start()
    {
        player = PlayerManager.instance.player;
        playerSkills = player.GetComponent<PlayerSkills>();
        skillsUI = battleUI.GetComponentsInChildren<Image>();
    }
    private void Update()
    {
        if (usedSkillIndex != -1 && enemy != null)
        {
            InitializeTurn();
        }
    }

    public void StartBattle(Camera battleCam, Camera roomCam, Enemy enemy)
    {
        this.enemy = enemy;
        battleUI.SetActive(true);
        roomCam.enabled = false;
        battleCam.enabled = true;
        player.GetComponent<PlayerController>().isInBattle = true;
        player.GetComponent<NavMeshAgent>().isStopped = true;
        foreach(Skill skill in playerSkills.activatedSkills)
        {
            skillsUI[playerSkills.activatedSkills.IndexOf(skill)].GetComponentInChildren<Text>().text = skill.name;
        }
        StartCoroutine(Sleep(1f));
        player.transform.position = new Vector3(battleCam.transform.position.x - 3.5f, player.transform.position.y, battleCam.transform.position.z + 10f);
        player.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
    public void InitializeTurn()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            return;
        }
        CheckForSpeed();
        //Move the player and enemy
        if (isPlayersTurn)
        {
            HandlePlayerStatusEffects();
            if (!IsStunned(player))
            {
                GetUsedSkill();
                if(playerSkills.activatedSkills[usedSkillIndex].skillTargets.Contains(SkillTarget.Enemy))
                {
                    playerSkills.activatedSkills[usedSkillIndex].UseSkillOnEnemy(enemy, player.GetComponent<PlayerStats>());
                }
                if (playerSkills.activatedSkills[usedSkillIndex].skillTargets.Contains(SkillTarget.Self))
                {
                    playerSkills.activatedSkills[usedSkillIndex].UseSkillOnSelf(player.GetComponent<PlayerStats>());
                }
                if (playerSkills.activatedSkills[usedSkillIndex].skillTargets.Contains(SkillTarget.Ally))
                {
                    playerSkills.activatedSkills[usedSkillIndex].UseSkillOnAlly(player.GetComponent<PlayerStats>());
                }
            }
            usedSkillIndex = -1;
            //Add resource
            player.GetComponent<PlayerStats>().currentResource += 10;
            Mathf.Clamp(player.GetComponent<PlayerStats>().currentResource, 0, player.GetComponent<PlayerStats>().maxResource.GetValue());
            
            HandleEnemyStatusEffects();
            if (!IsStunned(enemy.gameObject))
            {
                // move enemy
            }
            //Add resource
            enemy.GetComponent<EnemyStats>().currentResource += 10;
            Mathf.Clamp(enemy.GetComponent<EnemyStats>().currentResource, 0, enemy.GetComponent<EnemyStats>().maxResource.GetValue());
        }
        else
        {
            HandleEnemyStatusEffects();
            if (!IsStunned(enemy.gameObject))
            {
                // move enemy first
            }
            //Add resource
            enemy.GetComponent<EnemyStats>().currentResource += 10;
            Mathf.Clamp(enemy.GetComponent<EnemyStats>().currentResource, 0, player.GetComponent<EnemyStats>().maxResource.GetValue());

            HandlePlayerStatusEffects();
            if (!IsStunned(player))
            {
                GetUsedSkill();
                if (playerSkills.activatedSkills[usedSkillIndex].skillTargets.Contains(SkillTarget.Enemy))
                {
                    playerSkills.activatedSkills[usedSkillIndex].UseSkillOnEnemy(enemy, player.GetComponent<PlayerStats>());
                }
                if (playerSkills.activatedSkills[usedSkillIndex].skillTargets.Contains(SkillTarget.Self))
                {
                    playerSkills.activatedSkills[usedSkillIndex].UseSkillOnSelf(player.GetComponent<PlayerStats>());
                }
                if (playerSkills.activatedSkills[usedSkillIndex].skillTargets.Contains(SkillTarget.Ally))
                {
                    playerSkills.activatedSkills[usedSkillIndex].UseSkillOnAlly(player.GetComponent<PlayerStats>());
                }
            }
            usedSkillIndex = -1;
            //Add resource
            player.GetComponent<PlayerStats>().currentResource += 10;
            Mathf.Clamp(player.GetComponent<PlayerStats>().currentResource, 0, player.GetComponent<PlayerStats>().maxResource.GetValue());
        }
        //Check for status effects
    }
    public void GetUsedSkill()
    {
        usedSkillIndex = System.Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name.Substring(6, 1));
    }

    void CheckForSpeed()
    {
        if (player.GetComponent<PlayerStats>().speed.GetValue() > enemy.GetComponent<EnemyStats>().speed.GetValue())
        {
            isPlayersTurn = true;
        }
        else if (player.GetComponent<PlayerStats>().speed.GetValue() == enemy.GetComponent<EnemyStats>().speed.GetValue())
        {
            float randomizeTurn = Random.value;
            if (Random.value >= 0.5f)
            {
                isPlayersTurn = true;
            }
            else
            {
                isPlayersTurn = false;
            }
        }
        else if (player.GetComponent<PlayerStats>().speed.GetValue() > enemy.GetComponent<EnemyStats>().speed.GetValue())
        {
            isPlayersTurn = false;
        }
    }

    void HandlePlayerStatusEffects()
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
                enemy.GetComponent<EnemyStats>().TakeDamage(statusEffect.damage, DamageType.True, statusEffect.statusEffectType.ToString());
            }
            enemy.GetComponent<StatusEffectHandler>().CheckForRemoval(statusEffect);
        }
    }
    void HandleEnemyStatusEffects()
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
    bool IsStunned(GameObject target)
    {
        if (target.name == "Player")
        {
            foreach (StatusEffect statusEffect in player.GetComponent<StatusEffectHandler>().statusEffects.ToList())
            {
                if(statusEffect.statusEffectType == StatusEffectType.Stun)
                {
                    return true;
                }
            }
        }
        else if (target.name == "Enemy")
        {
            foreach (StatusEffect statusEffect in enemy.GetComponent<StatusEffectHandler>().statusEffects.ToList())
            {
                if (statusEffect.statusEffectType == StatusEffectType.Stun)
                {
                    return true;
                }
            }
        }
        return false;
    }

    IEnumerator Sleep(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
}
