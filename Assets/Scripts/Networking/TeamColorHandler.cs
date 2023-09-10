using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class TeamColorHandler : NetworkBehaviour
{
    [SerializeField] private Renderer[] colorRenderers = new Renderer[0];
    
    [SyncVar(hook = nameof(HandleTeamColorUpdated))]
    private Color _teamColor = new Color();

    #region Server
    
    public override void OnStartServer()
    {
        RTSPlayer player = connectionToClient.identity.GetComponent<RTSPlayer>();
        _teamColor = player.GetTeamColor();
    }
    
    #endregion
    
    #region Client
    
    private void HandleTeamColorUpdated(Color oldColor, Color newColor)
    {
        foreach (Renderer render in colorRenderers)
        {
            render.material.color = _teamColor;
            render.material.SetColor("_EmissionColor", _teamColor);
        }
    }
    
    #endregion
}
