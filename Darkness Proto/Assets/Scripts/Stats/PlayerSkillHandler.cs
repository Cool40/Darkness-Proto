using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    public List<Skill> warriorSkills;
    public List<Skill> rogueSkills;
    public List<Skill> mageSkills;
    public List<Skill> paladinSkills;
    private void Awake()
    {
        switch (GetComponent<PlayerStats>().playerClass)
        {
            case PlayerClass.Warrior:
                GetComponent<PlayerSkills>().activatedSkills = warriorSkills;
                break;
            case PlayerClass.Rogue:
                GetComponent<PlayerSkills>().activatedSkills = rogueSkills;
                break;
            case PlayerClass.Mage:
                GetComponent<PlayerSkills>().activatedSkills = mageSkills;
                break;
            case PlayerClass.Paladin:
                GetComponent<PlayerSkills>().activatedSkills = paladinSkills;
                break;
        }
    }
}
