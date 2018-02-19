using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : ScriptableObject {

    public StatusEffectType statusEffectType;
    public int damage;
    public int duration;
    public int remainingDuration;
}

public enum StatusEffectType { Bleed, Blight, Burn, Stun }
