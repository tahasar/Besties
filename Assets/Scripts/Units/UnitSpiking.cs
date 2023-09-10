using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnipSpiking : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject spikejectilePrefab = null;
    [SerializeField] private Transform spikejectileSpawnPoint = null;
    [SerializeField] private float fireRange = 2f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 20f;
    private RtsNetworkManager _networkManager = null;
    [SerializeField] private List<Targeter> targeters = new List<Targeter>();

    private Targetable _target = null;

    private float _lastFireTime;

    private void Start()
    {
        _networkManager = GameObject.Find("NetworkManager").GetComponent<RtsNetworkManager>();
    }

    [ServerCallback] //
    private void Update()
    {
        foreach (GameObject unit in _networkManager.unitList)
        {
            if (unit.GetComponent<NetworkIdentity>().connectionToClient == connectionToClient)
            {
                continue;
            }
            
            if (Mathf.Abs(Vector3.Distance(unit.transform.position, transform.position)) <= fireRange * fireRange)
            {
                Debug.Log("Menzilde");
                Targeter unitToAttack = unit.GetComponent<Targeter>();
                targeters.Add(unitToAttack);
            }else
            {
                Debug.Log("Menzilde degil");
                targeters.Remove(unit.GetComponent<Targeter>());
            }
        }
        
        

        if (Time.time > (1 / fireRate) + _lastFireTime)
        {
            foreach (Targeter target in targeters)
            {
                if (target == null)
                {
                    continue;
                }
                target.GetComponent<Health>().DealDamage(70);
                
                #region old code
                //var currentTarget = target.GetTarget();
                //
                //Quaternion targetRotation =
                //    Quaternion.LookRotation(currentTarget.transform.position - transform.position);
                //Debug.Log("target rotation");
                //
                //transform.rotation = Quaternion.RotateTowards(
                //    transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                //Debug.Log("transform rotation");
                //
                //Debug.Log("time is greater than fire rate");
                //Quaternion projectileRotation = Quaternion.LookRotation(
                //    currentTarget.GetAimAtPoint().position - spikejectileSpawnPoint.position);
                //
                //GameObject projectileInstance = Instantiate(
                //    spikejectilePrefab, spikejectileSpawnPoint.position, projectileRotation);
                //
                //NetworkServer.Spawn(projectileInstance, connectionToClient);
                #endregion
            }
            
            _lastFireTime = Time.time;
        }

    }
    
    public void GetTarget()
    {
        _target = targeter.GetTarget();
    }

    [Server]
    private bool CanFireAtTarget()
    {
        return (_target.transform.position - transform.position).sqrMagnitude
               <= fireRange * fireRange;
    }
}