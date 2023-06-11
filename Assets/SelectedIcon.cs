using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class SelectedIcon : MonoBehaviour
{
    private Transform unit;
    private void Start()
    {
        unit = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,80 * Time.deltaTime,0);
    }
}
