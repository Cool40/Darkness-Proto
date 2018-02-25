using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{

    public BattleManager battleManager;
    private Camera battleCam;
    private Camera roomCam;

    private void Start()
    {
        battleCam = GetComponentInChildren<Camera>();
        roomCam = transform.parent.GetComponentInChildren<Camera>();
        battleManager = BattleManager.instance;
    }

    private void OnTriggerEnter(Collider collider)
    {
        battleManager.StartBattle(battleCam, roomCam, GetComponentsInChildren<Enemy>());
    }
}
