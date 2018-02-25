using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {

    [SerializeField]
    private float baseValue;


    private List<float> modifiers = new List<float>();
    private List<float> percentageModifiers = new List<float>();

    public float GetValue()
    {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        percentageModifiers.ForEach(x => finalValue *= (x + 1));
        return finalValue;
    }
    public void AddModifier (float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }
    public void AddPercentageModifier(float modifier)
    {
        if (modifier != 0)
        {
            percentageModifiers.Add(modifier);
        }
    }
    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
    public void RemovePercentageModifier(float modifier)
    {
        if (modifier != 0)
        {
            percentageModifiers.Remove(modifier);
        }
    }
}
