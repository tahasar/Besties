using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class UnitMovement : MonoBehaviour
{
    private Camera myCam;

    private NavMeshAgent myAgent;

    public LayerMask ground;
    
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
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity,ground))
            {
                myAgent.SetDestination(hit.point);
            }
        }
    }
}
