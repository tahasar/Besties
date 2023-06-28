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
    [SerializeField] private HealthBar _healthbar;

    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeselect = null;
    // Health and Attack Stats
    [SerializeField] private int maxHealth = 100;
    [SyncVar(hook = nameof(HandleHealthUpdated))] private int currentHealth;

    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime = 0f;

    [SerializeField] private string unitName;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;
    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #region Server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;
        //_healthbar.UpdateHealthBar(maxHealth,currentHealth);
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }

    [Server]
    public void DealDamage(Unit target)
    {
        if (target == null || !target.hasAuthority)
            return;
        
        if (!CanAttack())
            return;
        
        target.TakeDamage(attackDamage);
        lastAttackTime = Time.time;
    }

    [Server]
    private void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0)
            return;
        
        currentHealth -= damageAmount;
        
        if (currentHealth <= 0)
        {
            // Unit destroyed
            NetworkServer.Destroy(gameObject);
        }

        
    }

    private void HandleHealthUpdated(int oldValue, int newValue)
    {
        //_healthbar.UpdateHealthBar(maxHealth,currentHealth);

    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
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
