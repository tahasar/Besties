using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public Team team;

    [SerializeField] private Player[] players;

    public static Player Instance { get { return _instance; } }

    private static Player _instance;

    private void Awake()
    {
        _instance = this;
    }

    public override void OnStartClient()
    {
        players = FindObjectsOfType<Player>();

        if (players.Length == 1)
        {
            Debug.Log("0 oyuncu vardı ve blue renk verildi.");
            team = Team.Blue;
        }
        else if (players.Length == 2)
        {
            if (players[0].team == Team.Blue)
            {
                Debug.Log("2  oyuncu vardı ve blue renk verildi.");
                team = Team.Red;
            }else if (players[0].team == Team.Red)
            {
                Debug.Log("2  oyuncu vardı ve red renk verildi.");
                team = Team.Blue;
            }
        }
    }
}
