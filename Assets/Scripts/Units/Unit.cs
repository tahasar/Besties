using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselect = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;
    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);

    }
    #endregion
    public UnitMovement GetUnitMovement()
    {
        return unitMovement;
    }
    
    public Targeter GetTargeter()
    {
        return targeter;
    }

    #region Client

    public override void OnStartClient()
    {
        if (!isClientOnly || !isOwned)
        {
            return;
        }
        AuthorityOnUnitSpawned?.Invoke(this);
    }
    
    public override void OnStopClient()
    {
        if (!isClientOnly || !isOwned)
        {
            return;
        }
        
        AuthorityOnUnitDespawned?.Invoke(this);
    }

    public void Select()
    {
        if (!isOwned)
            return;
        
        onSelected?.Invoke();
    }

    public void Deselect()
    {
        if (!isOwned)
            return;
        
        onDeselect?.Invoke();
    }

    #endregion
}
