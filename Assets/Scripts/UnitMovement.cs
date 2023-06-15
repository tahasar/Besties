using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Photon.Realtime;
using Photon.Pun;

public class UnitMovement : MonoBehaviourPun
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
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(1))
            {
                unitsToFormation = UnitSelections.Instance.unitsSelected;

                RaycastHit hit;
                Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    if (unitsToFormation.Count > 1)
                    {
                        // Update the positions of all units in the formation
                        for (int i = 0; i < unitsToFormation.Count; i++)
                        {
                            var angle = i * Mathf.PI * (2 * _rotations) / unitsToFormation.Count + (i % 2 != 0 ? _nthOffset : 0);

                            var radius = _radius + _ringOffset + i * _radiusGrowthMultiplier;
                            var x = Mathf.Cos(angle) * radius;
                            var z = Mathf.Sin(angle) * radius;

                            var pos = new Vector3(hit.point.x + x, 0, hit.point.z + z);

                            // Update the position of the unit on all other clients
                            photonView.RPC("UpdatePosition", RpcTarget.All, pos, photonView.ViewID, i);

                            // Update the position of the unit on the local client
                            unitsToFormation[i].transform.position = pos;
                        }
                    }
                    else
                    {
                        // Update the position of the unit on all other clients
                        photonView.RPC("UpdatePosition", RpcTarget.All, hit.point, photonView.ViewID, 0);

                        // Update the position of the unit on the local client
                        myAgent.SetDestination(hit.point);
                    }
                }
            }
        }
    }

    [PunRPC]
    void UpdatePosition(Vector3 position, int viewID, int unitIndex)
    {
        GameObject unit = PhotonView.Find(viewID).gameObject;
        unit.transform.position = position;
    }
}
