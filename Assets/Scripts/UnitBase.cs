using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UnitBase", menuName = "Unit")]
public class UnitBase : ScriptableObject
{
    public Team team;
    public float health;
    public float attackDamage;
}

public enum Team
{
    Blue,
    Red,
}
