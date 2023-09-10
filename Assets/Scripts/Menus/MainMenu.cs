using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using Mirror;
using Mirror.FizzySteam;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Steamworks;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject landingPagePanel = null;
    
    [SerializeField] private bool useSteam = false;

    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> GameLobbyJoinRequested;
    protected Callback<LobbyEnter_t> LobbyEntered;

    private void Start()
    {
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        GameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobbyWithoutSteam()
    {   
        useSteam = false;
        NetworkManager.singleton.transport = NetworkManager.singleton.GetComponent<KcpTransport>();

        landingPagePanel.SetActive(false);

        NetworkManager.singleton.StartHost();
    }
    
    public void HostLobbyWithSteam()
    {
        useSteam = true;
        NetworkManager.singleton.transport = NetworkManager.singleton.GetComponent<FizzySteamworks>();
        
        
        landingPagePanel.SetActive(false);

        SteamMatchmaking.CreateLobby(
            ELobbyType.k_ELobbyTypeFriendsOnly,
            NetworkManager.singleton.maxConnections);
    }
    
    public void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            landingPagePanel.SetActive(true);
            
            return;
        }
        
        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "HostAddress",
            SteamUser.GetSteamID().ToString());
        
        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "DisplayName",
            SteamUser.GetPlayerSteamLevel().ToString());
        
        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "UseSteam",
            useSteam.ToString());
        
        NetworkManager.singleton.StartHost();
    }
    
    public void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
    {
        NetworkManager.singleton.StartClient();

        landingPagePanel.SetActive(false);
    }
    
    public void OnLobbyEntered(LobbyEnter_t callback)
    {
        if (NetworkServer.active) { return; }
        
        string hostAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "HostAddress");
        
        NetworkManager.singleton.networkAddress = hostAddress;
        NetworkManager.singleton.StartClient();
        
        landingPagePanel.SetActive(false);
    }
}