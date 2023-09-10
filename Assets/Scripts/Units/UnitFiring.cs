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

    private Targetable _target = null;

    private float _lastFireTime;

    [ServerCallback]
    private void Update()
    {
        _target = targeter.GetTarget();

        if (_target == null) { return; }
        Debug.Log("_target is not null");

        if (!CanFireAtTarget()) { return; }
        Debug.Log("can fire at _target");

        Quaternion targetRotation =
            Quaternion.LookRotation(_target.transform.position - transform.position);
        Debug.Log("_target rotation");

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        Debug.Log("transform rotation");

        if (Time.time > (1 / fireRate) + _lastFireTime)
        {
            Debug.Log("time is greater than fire rate");
            var position = projectileSpawnPoint.position;
            Quaternion projectileRotation = Quaternion.LookRotation(
                _target.GetAimAtPoint().position - position);

            GameObject projectileInstance = Instantiate(
                projectilePrefab, position, projectileRotation);

            NetworkServer.Spawn(projectileInstance, connectionToClient);

            _lastFireTime = Time.time;
        }
    }

    [Server]
    private bool CanFireAtTarget()
    {
        
        return (_target.transform.position - transform.position).sqrMagnitude
               <= fireRange * fireRange;
    }
}