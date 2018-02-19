using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public BattleManager battleManager;
    private Camera battleCam;
    private Camera roomCam;
    public MonsterType monsterSpecies;

    void Start()
    {
        battleCam = transform.parent.GetComponentInChildren<Camera>();
        roomCam = transform.parent.parent.GetComponentInChildren<Camera>();
        battleManager = BattleManager.instance;
    } 

    private void OnTriggerEnter(Collider collider)
    {
        battleManager.StartBattle(battleCam, roomCam, this);
    }
}
