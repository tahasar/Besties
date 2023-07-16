using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameDisplay : MonoBehaviour
{
    [SerializeField] private GameObject nameDisplay = null;

    private void OnMouseEnter()
    {
        nameDisplay.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        nameDisplay.SetActive(false);//
    }
}
