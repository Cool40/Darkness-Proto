using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour {
    public int currentLevel;
    int experience;
    int requiredExperience;

    void AddExperience(int value)
    {
        experience += value;
        LevelUp();
        Mathf.Clamp(experience, 0, requiredExperience);
    }

    void LevelUp()
    {
        if (experience >= requiredExperience && currentLevel < 50)
        {
            currentLevel++;
            if (experience == requiredExperience)
            {
                currentLevel++;
                experience = 0;
        }
            else if (experience > requiredExperience)
            {
                experience -= requiredExperience;
            }
            requiredExperience = (int)Mathf.Round(50 + Mathf.Pow(currentLevel, 1.5f) * 10);
        }
    }
}
