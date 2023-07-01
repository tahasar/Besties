using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Building : NetworkBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int id = -1;
    [SerializeField] private int price = 100;
    
    public static event System.Action<Building> ServerOnBuildingSpawned;
    public static event System.Action<Building> ServerOnBuildingDespawned;
    
    public static event System.Action<Building> AuthorityOnBuildingSpawned;
    public static event System.Action<Building> AuthorityOnBuildingDespawned;
    
    public Sprite GetIcon()
    {
        return icon;
    }
    
    public int GetId()
    {
        return id;
    }
    
    public int GetPrice()
    {
        return price;
    }
    
    #region Server
    
    
    public override void OnStartServer()
    {
        health.ServerOnDie += ServerHandleDie;
        
        ServerOnBuildingSpawned?.Invoke(this);
    }
    
    public override void OnStopServer()
    {
        ServerOnBuildingDespawned?.Invoke(this);
        
        health.ServerOnDie -= ServerHandleDie;
    }

    private void ServerHandleDie()
    {
        throw new System.NotImplementedException();
    }
    
    #endregion
    
    #region Client
    
    
    public override void OnStartAuthority()
    {
        AuthorityOnBuildingSpawned?.Invoke(this);
    }
    
    public override void OnStopClient()
    {
        if (!hasAuthority) { return; }
        
        AuthorityOnBuildingDespawned?.Invoke(this);
    }
    
    #endregion
}
