using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerSkills : MonoBehaviour {

    public List<Skill> activatedSkills = new List<Skill>();
    public List<Skill> disactivatedSkills = new List<Skill>();

    private void Update()
    {
        if(activatedSkills != null)
        {
            foreach (Skill skill in activatedSkills.ToList())
            {
                if (!skill.isActivated)
                {
                    disactivatedSkills.Add(skill);
                    activatedSkills.Remove(skill);
                }
            }
        }
        if(disactivatedSkills != null)
        {
            foreach (Skill skill in disactivatedSkills.ToList())
            {
                if (skill.isActivated)
                {
                    activatedSkills.Add(skill);
                    disactivatedSkills.Remove(skill);
                }
            }
        }
    }
}
