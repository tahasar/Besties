using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string gameVersion = "0.0.0";
    public string GameVersion => gameVersion;

    [SerializeField] public string _nickname = "Astorat";

    public string Nickname
    {
        get
        {
            int value = Random.Range(0, 9999);
            return _nickname + value.ToString();
        }
    }
}
