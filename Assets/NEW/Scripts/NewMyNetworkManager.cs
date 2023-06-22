using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NewMyNetworkManager : NetworkManager
{
    public override void OnClientConnect()
    {
        base.OnClientConnect();
        
        Debug.Log("I connected to a server.");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient connection)
    {
        base.OnServerAddPlayer(connection);

        NewMyNetworkPlayer player = connection.identity.GetComponent<NewMyNetworkPlayer>();

        player.SetDisplayName($"Player {numPlayers}");

        Debug.Log($"There are now {numPlayers} players.");

        if (numPlayers == 1)
            player.SetTeamColor(Color.blue);
        else if (numPlayers == 2)
            player.SetTeamColor(Color.red);
    }
}
