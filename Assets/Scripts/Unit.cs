using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Unit : MonoBehaviourPunCallbacks
{
    public UnitBase unitBase;
    public Team team;
    public float health;
    public float attackDamage;

    // Start is called before the first frame update
    

    public override void OnJoinedRoom()
    {
        team = unitBase.team;
        health = unitBase.health;
        attackDamage = unitBase.attackDamage;
        UnitSelections.Instance.unitList.Add(this.gameObject);
    }

    public void Attack()
    {
        
    }

    private void OnDestroy()
    {
        UnitSelections.Instance.unitsSelected.Remove(this.gameObject);
        UnitSelections.Instance.unitList.Remove(this.gameObject);
    }
}
