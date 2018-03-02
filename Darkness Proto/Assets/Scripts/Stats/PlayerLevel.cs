using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour {
    public int currentLevel;
    public int experience;
    int requiredExperience;

    private void Start()
    {
        requiredExperience = (int)Mathf.Round(50 + Mathf.Pow(currentLevel, 1.5f) * 10);
    }

    public void AddExperience(int value)
    {
        experience += value;
        LevelUp();
        Mathf.Clamp(experience, 0, requiredExperience);
    }

    void LevelUp()
    {
        if (experience >= requiredExperience && currentLevel < 50)
        {
            Debug.Log(name + " levels up to level " + currentLevel + 1 + "!");
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
