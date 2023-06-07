using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkTransformTest : NetworkBehaviour
{

    public Transform parent;
    private void Update()
    {
        if (IsOwner && IsServer)
        {
            transform.RotateAround(parent.position, Vector3.up, 100 * Time.deltaTime);
        }
    }
}
