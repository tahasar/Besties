using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    private static UnitSelections _instance;

    public static UnitSelections Instance { get { return _instance; } }

    public Vector3 averagePosition;
    private int unitWidth;
    private int unitDepth;

    private PhotonView view;

    public bool isMine;

    private void Start()
    {
        if (_instance !=null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        view = GetComponentInParent<PhotonView>();
    }

    private void Update()
    {
        if (view.IsMine)
        {
            isMine = true;
        }
        
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        if (isMine)
        {
            DeselectAll();
            unitsSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }
    
    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if (isMine)
        {
            if (!unitsSelected.Contains(unitToAdd))
            {
                unitsSelected.Add(unitToAdd);
                unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
                unitToAdd.GetComponent<UnitMovement>().enabled = true;
            }
            else
            {
                unitToAdd.GetComponent<UnitMovement>().enabled = false;
                unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
                unitsSelected.Remove(unitToAdd);

            }
        }
    }
    
    public void DragSelect(GameObject unitToAdd)
    {
        if (isMine)
        {
            if (!unitsSelected.Contains(unitToAdd))
            {
                unitsSelected.Add(unitToAdd);
                unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
                unitToAdd.GetComponent<UnitMovement>().enabled = true;
            }
        }
    }

    public void DeselectAll()
    {
        if (isMine)
        {
            foreach (var unit in unitsSelected)
            {
                unit.GetComponent<UnitMovement>().enabled = false;
                unit.transform.GetChild(0).gameObject.SetActive(false);
            }
            unitsSelected.Clear();
        }
    }

    public void Deselect(GameObject unitToDeselect)
    {
        
    }

    public Vector3 CalculateCenterOfSelectedUnits()
    {
        Vector3 totalPosition = Vector3.zero;

        foreach (var unit in unitsSelected)
        {
            totalPosition += unit.transform.position;
        }

        averagePosition = totalPosition / unitsSelected.Count;

        return averagePosition;
    }
}
