using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class CubeMovement : NetworkBehaviour
{

    #region Server

    [Command]
    public void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            return;}

        
        
    }

    #endregion

    #region Client
    
    
    void Update()
    {
        if (isOwned)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 movement = new Vector3(horizontal * 0.1f, 0,vertical * 0.1f);
            transform.position = transform.position + movement;
        }

    }

    #endregion

}