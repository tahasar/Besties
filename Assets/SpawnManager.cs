using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SpawnManager : NetworkBehaviour, IPointerClickHandler
{
    private IPointerClickHandler _pointerClickHandlerImplementation;

    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform unitSpawnPoint = null;

    [Command]
    private void CmdSpawnUnit()
    {
        Debug.Log("ara");
        GameObject unitSpawn = Instantiate(unitPrefab, unitSpawnPoint.position, unitSpawnPoint.rotation);

        NetworkServer.Spawn(unitSpawn, connectionToClient);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }

        if (!isOwned)
        {
            return;
        }
        
        CmdSpawnUnit();
    }
}