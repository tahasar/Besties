using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{

    [SerializeField] private NavMeshAgent agent = null;

    public Camera mainCamera;
    public GameObject blueUnit = null;
    public GameObject redUnit = null;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    #region Server

    [Command]
    public void CmdMove(Vector3 position)
    {

    }

    #endregion

    #region Client
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Blue Unit oluşturuldu.");
            SpawnManager.Instance.SpawnObject(blueUnit,new Vector3(0,3,-4));
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Red Unit oluşturuldu.");
            SpawnManager.Instance.SpawnObject(redUnit, new Vector3(0,3,4));
        }
    }
    

    #endregion

    /*
        unitsToFormation = UnitSelections.Instance.unitsSelected;

        RaycastHit hit;
        Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
        {
            if (unitsToFormation.Count > 1)
            {
                var amountPerRing = unitsToFormation.Count / _rings;
                var ringOffset = 2f;

                if (unitsToFormation.Count % _rings != 0)
                {
                    amountPerRing++;
                    ringOffset += _ringOffset;
                }

                for (int i = 0; i < _rings; i++)
                {
                    for (var j = 0; j < amountPerRing; j++)
                    {
                        var angle = j * Mathf.PI * (2 * _rotations) / amountPerRing + (i % 2 != 0 ? _nthOffset : 0);

                        var radius = _radius + ringOffset + j * _radiusGrowthMultiplier;
                        var x = Mathf.Cos(angle) * radius;
                        var z = Mathf.Sin(angle) * radius;

                        var pos = new Vector3(hit.point.x + x, 0, hit.point.z + z);


                        unitsToFormation[j].GetComponent<NavMeshAgent>().SetDestination(pos);
                    }

                    ringOffset += _ringOffset;
                }
            }
            else
            {
                myAgent.SetDestination(hit.point);
            }
        }
        */
}