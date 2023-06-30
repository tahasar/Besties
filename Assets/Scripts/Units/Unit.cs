using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatMagnitude = 0.5f;
    private float phaseOffset;
    private Vector3 startPosition;


    [SerializeField] private Health health = null;
    [SerializeField] private UnitMovement unitMovement = null;
    [SerializeField] private Targeter targeter = null;

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
        
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        
        ServerOnUnitDespawned?.Invoke(this);
    }
    
    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Server]
    public void DealDamage(Unit target)
    {
        if (target == null || !target.isOwned)
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

    public override void OnStartAuthority()
    {

        AuthorityOnUnitSpawned?.Invoke(this);
        phaseOffset = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        startPosition = transform.position;    }

    public override void OnStopClient()
    {
        if (!isOwned)
        {
            return;
        }

        AuthorityOnUnitDespawned?.Invoke(this);
    }

    private void Update()
    {
        if (hasAuthority)
        {
           float newY = startPosition.y + Mathf.Sin((Time.time + phaseOffset) * floatSpeed) * floatMagnitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
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
