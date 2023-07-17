using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitFiring : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectileSpawnPoint = null;
    [SerializeField] private float fireRange = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 20f;

    private Targetable target = null;

    private float lastFireTime;

    [ServerCallback]
    private void Update()
    {
        target = targeter.GetTarget();

        if (target == null) { return; }
        Debug.Log("target is not null");

        if (!CanFireAtTarget()) { return; }
        Debug.Log("can fire at target");

        Quaternion targetRotation =
            Quaternion.LookRotation(target.transform.position - transform.position);
        Debug.Log("target rotation");

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        Debug.Log("transform rotation");

        if (Time.time > (1 / fireRate) + lastFireTime)
        {
            Debug.Log("time is greater than fire rate");
            Quaternion projectileRotation = Quaternion.LookRotation(
                target.GetAimAtPoint().position - projectileSpawnPoint.position);

            GameObject projectileInstance = Instantiate(
                projectilePrefab, projectileSpawnPoint.position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            lastFireTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        
        return (target.transform.position - transform.position).sqrMagnitude
               <= fireRange * fireRange;
    }
}