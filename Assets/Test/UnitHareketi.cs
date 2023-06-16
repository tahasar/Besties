using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitHareketi : NetworkBehaviour
{

    [SerializeField] private NavMeshAgent agent = null;

    private Camera mainCamera;

    #region Server

    [Command]
    public void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return;}

        agent.SetDestination(hit.position);
        
    }

    #endregion

    #region Client

    [ClientCallback]
    void Update()
    {
        if (!isOwned)
        {
            return;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return;}
        
        CmdMove(hit.point);

    }
    
    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;

    }

    #endregion

}
