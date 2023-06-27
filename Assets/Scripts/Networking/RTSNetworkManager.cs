using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawnerPrefab = null;
    
    
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        
        Debug.Log("I connected to a server.");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient connection)
    {
        base.OnServerAddPlayer(connection);

        RTSPlayer player = connection.identity.GetComponent<RTSPlayer>();
        
        player.SetTeamColor(new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)));
        
        
        GameObject unitSpawnerInstantiate = Instantiate(
            unitSpawnerPrefab,
            connection.identity.transform.position,
            connection.identity.transform.rotation);
        
        NetworkServer.Spawn(unitSpawnerInstantiate, connection);

        //Player player = connection.identity.GetComponent<Player>();

        //player.SetDisplayName($"Player {numPlayers}");

        //Debug.Log($"There are now {numPlayers} players.");

        //if (numPlayers == 1)
        //    player.SetTeamColor(Color.blue);
        //else if (numPlayers == 2)
        //    player.SetTeamColor(Color.red);
    }
}
