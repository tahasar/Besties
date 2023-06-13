using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitBase unitBase;
    public Team team;
    public float health;
    public float attackDamage;

    private void OnValidate()
    {
        team = unitBase.team;
        health = unitBase.health;
        attackDamage = unitBase.attackDamage;
    }

    // Start is called before the first frame update
    void Start()
    {
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
