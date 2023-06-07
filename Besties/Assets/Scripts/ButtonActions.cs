using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ButtonActions : MonoBehaviour
{
    private NetworkManager NetworkManager;

    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager = GetComponentInParent<NetworkManager>();
    }

    public void StartHost()
    {
        NetworkManager.StartHost();
        InitMovementText();
    }

    public void StartClient()
    {
        NetworkManager.StartClient();
        InitMovementText();
    }

    public void SubmitNewPosition()
    {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        var player = playerObject.GetComponent<PlayerMovement>();
        player.Move();
    }

    private void InitMovementText()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            text.text = "MOVE";
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            text.text = "Request Move";
        }
    }
}
