using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    [SerializeField] private Targetable target;
    
    public Targetable GetTarget()
    {
        return target;
    }
    
    #region Server
    
    [Command]
    public void CmdSetTarget(GameObject targetGameObject)
    {
        if (!targetGameObject.TryGetComponent<Targetable>(out Targetable target)) { return; }

        this.target = target;
    }
    
    [Server]
    public void ClearTarget()
    {
        target = null;
    }
    
    #endregion
}
