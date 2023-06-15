using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI logText;
    #region MyRegion

    // Start is called before the first frame update
    void Start()
    { 
        print("Connecting to server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        //logText.text = (PhotonNetwork.NickName+ ", " + PhotonNetwork.GameVersion + ", " + PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.LocalPlayer.NickName = PhotonNetwork.LocalPlayer.UserId;
        SceneManager.LoadScene("Lobby");
        Debug.Log("connected.");
        print("Connected to server.");
        print(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnecting from server for " + cause.ToString());
    }

    #endregion
}
