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


    private void Start()
    {
        nameText.text = unitName;
    }

    public void UpdateUnitName(string newName)
    {
        unitName = newName;
        nameText.text = unitName;
    }
}
