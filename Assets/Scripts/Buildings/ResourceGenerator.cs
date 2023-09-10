using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class ResourceGenerator : NetworkBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private int resourcesPerInterval = 10;
    [SerializeField] private float interval = 2f;
    private float _timer;
    private RTSPlayer _player;
    
    public override void OnStartServer()
    {
        _timer = interval;
        _player = connectionToClient.identity.GetComponent<RTSPlayer>();
        
        health.ServerOnDie += ServerHandleDie;
        GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }
    
    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;//
    }
    
    [ServerCallback]
    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            _player.SetResources(_player.GetResources() + resourcesPerInterval);
            
            _timer += interval;
        }
    }
    
    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }
    
    [Server]
    private void ServerHandleGameOver()
    {
        enabled = false;
    }
}
