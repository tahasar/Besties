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
    
    [SerializeField] private float _radius = 1;
    [SerializeField] private float _radiusGrowthMultiplier = 0;
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
                if(unitsToFormation.Count > 1)
                {
                    var amountPerRing = unitsToFormation.Count / _rings;
                    var ringOffset = 2f;
                    
                    for (int i = 0; i < _rings; i++) {
                        
                        Debug.Log("i kısmı");
                        
                        for (var j = 0; j < amountPerRing; j++)
                        {
                            Debug.Log("j kısmı");
                            var angle = j * Mathf.PI * (2 * _rotations) / amountPerRing + (i % 2 != 0 ? _nthOffset : 0);

                            var radius = _radius + ringOffset + j * _radiusGrowthMultiplier;
                            var x = Mathf.Cos(angle) * radius;
                            var z = Mathf.Sin(angle) * radius;

                            var pos = new Vector3(hit.point.x + x, 0, hit.point.z + z);
                                
                            //var angle = Mathf.PI * 2 * angleStep * i;
                        
                            //Debug.Log($"unit.{i} > {angle}");
                        
                            //var z = Mathf.Cos(angle) * 5;
                            //var x = Mathf.Sin(angle) * 5;

                            //var pos = new Vector3(hit.point.x + x, 0, hit.point.z + z);

                            unitsToFormation[j].GetComponent<NavMeshAgent>().SetDestination(pos);
                        }

                        ringOffset += _ringOffset;
                    }
                    
                    //float angleStep = 360 / unitsToFormation.Count - 1;
                    //angleStep = Mathf.RoundToInt(angleStep);
//
                    //int i;
                    //for (i = 0; i < unitsToFormation.Count; i++) // ikinci unit'ten başlar ve altıncı unit ile ilk halkayı oluşturur.
                    //{
                    //    
                    //    if (i == 0) // ilk unit merkeze konumlanır.
                    //    {
                    //        unitsToFormation[i].GetComponent<NavMeshAgent>().SetDestination(hit.point);
                    //        continue;
                    //    }
                    //    
                    //    //////////////////////////////////////////////////////////////////////////3
                    //    var amountPerRing = _amount / _rings;
                    //    var ringOffset = 0f;
                    //    
                    //    for (i = 0; i < _rings; i++) {
                    //        for (var j = 0; j < amountPerRing; j++)
                    //        {
                    //            var angle = j * Mathf.PI * (2 * _rotations) / amountPerRing + (i % 2 != 0 ? _nthOffset : 0);
//
                    //            var radius = _radius + ringOffset + j * _radiusGrowthMultiplier;
                    //            var x = Mathf.Cos(angle) * radius;
                    //            var z = Mathf.Sin(angle) * radius;
//
                    //            var pos = new Vector3(x, 0, z);
                    //            
                    //            //var angle = Mathf.PI * 2 * angleStep * i;
                    //    
                    //            //Debug.Log($"unit.{i} > {angle}");
                    //    
                    //            //var z = Mathf.Cos(angle) * 5;
                    //            //var x = Mathf.Sin(angle) * 5;
//
                    //            //var pos = new Vector3(hit.point.x + x, 0, hit.point.z + z);
//
                    //            unitsToFormation[i].GetComponent<NavMeshAgent>().SetDestination(pos);
                    //        }
//
                    //        ringOffset += _ringOffset;
                    //    }
                        //////////////////////////////////////////////////////////////////////////
                        /// 
                        
                        
                        //Mathf.PI * (2 * _rotations) / amountPerRing;
                        //unitsToFormation[i].transform.localPosition = new Vector3(hit.point.x + 1, unitsToFormation[i].transform.position.y, hit.point.z);
                }
                else
                {
                    myAgent.SetDestination(hit.point);
                }
            }
        }
    }
}
