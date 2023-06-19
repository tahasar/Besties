using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    
    GameObject objectToSpawn = null;
    Vector3? spawnPosition = null;
    
    #region Singleton

    private static SpawnManager _instance;
    public static SpawnManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance !=null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    //[Command]
    //public void SpawnObject(GameObject objectToSpawn,Vector3 spawnPosition)
    //{
    //    GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
    //    NetworkServer.Spawn(spawnedObject, connectionToClient);
    //}
}
