using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public void LeaveGame()
    {
        NetworkManager.singleton.StopClient();
        
        SceneManager.LoadScene("Lobby");
    }
}
