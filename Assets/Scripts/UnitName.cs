using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitName : MonoBehaviour
{
    [SerializeField]
    private string unitName;

    [SerializeField]
    private TMP_Text nameText; 


private Camera _cam;

    
    private void Start()
    {
         _cam = Camera.main;
        nameText.text = unitName;
    }

    public void UpdateUnitName(string newName)
    {
        unitName = newName;
        nameText.text = unitName;
    }

    void Update(){
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
