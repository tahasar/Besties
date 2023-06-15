using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class UnitMovement : MonoBehaviourPun, IPunObservable
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

    // Define team colors or IDs
    public enum Team { Red, Blue };
    public Team unitTeam;

    // Start is called before the first frame update
    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            unitsToFormation = UnitSelections.Instance.unitsSelected;

            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                if (unitTeam == Team.Red)
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

                                photonView.RPC("SetDestinationRPC", RpcTarget.All, pos, j);
                            }

                            ringOffset += _ringOffset;
                        }
                    }
                    else
                    {
                        photonView.RPC("SetDestinationRPC", RpcTarget.All, hit.point, -1);
                    }
                }
                else if (unitTeam == Team.Blue)
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

                                photonView.RPC("SetDestinationRPC", RpcTarget.All, pos, j);
                            }

                            ringOffset += _ringOffset;
                        }
                    }
                    else
                    {
                        photonView.RPC("SetDestinationRPC", RpcTarget.All, hit.point, -1);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void SetDestinationRPC(Vector3 destination, int index)
    {
        if (index >= 0 && index < unitsToFormation.Count)
        {
            unitsToFormation[index].GetComponent<NavMeshAgent>().SetDestination(destination);
        }
        else if (index == -1)
        {
            myAgent.SetDestination(destination);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
        }
    }
}
