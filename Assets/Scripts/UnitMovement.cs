using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class UnitMovement : MonoBehaviour
{
    private Camera myCam;

    private NavMeshAgent myAgent;

    public LayerMask ground;

    private List<GameObject> unitsToFormation = new List<GameObject>();
    
    [SerializeField] private int _radius = 1;
    [SerializeField] private int _radiusGrowthMultiplier = 0;
    [SerializeField] private float _rotations = 1;
    [SerializeField] private int _rings = 1;
    [SerializeField] private float _ringOffset = 1;
    [SerializeField] private float _nthOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            unitsToFormation = UnitSelections.Instance.unitsSelected;
            
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity,ground))
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
                    
                    for (int i = 0; i < _rings; i++) {
                        
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
        }
    }
}
