using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private Camera mainCamera;
    public NavMeshAgent myAgent;
    
    [SerializeField] private List<GameObject> unitsToFormation = new List<GameObject>();
    [SerializeField] private int ground;
    [SerializeField] private int _rings = 1;
    [SerializeField] private float _ringOffset = 2;
    [SerializeField] private int _rotations = 1;
    [SerializeField] private int _radiusGrowthMultiplier = 1;
    [SerializeField] private float _radius = 5;
    [SerializeField] private float _nthOffset = 1;

    #region Singleton

    private static UnitMovement _instance;

    public static UnitMovement Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;
    }
    #endregion
    
    private void Start()
    {
        mainCamera = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    #region Server

    private void Update()
    {
        if (!isOwned)
        {
            return;
        }
        
        if (Mouse.current.rightButton.isPressed)
        {
            CmdMove();
        }
    }

    public void CmdMove()
    {
        myAgent = GetComponent<NavMeshAgent>();

        unitsToFormation = UnitSelections.Instance.unitsSelected;
        
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
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

                        var pos = new Vector3(hit.point.x + x, unitsToFormation[j].transform.position.y, hit.point.z + z);


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
    }

    #endregion
}