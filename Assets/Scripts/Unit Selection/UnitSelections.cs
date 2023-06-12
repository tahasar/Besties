using System;
using System.Collections.Generic;
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

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SetFormation();
        }
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        unitsSelected.Add(unitToAdd);
        unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
        unitToAdd.GetComponent<UnitMovement>().enabled = true;
    }
    
    public void ShiftClickSelect(GameObject unitToAdd)
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
    
    public void DragSelect(GameObject unitToAdd)
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.transform.GetChild(0).gameObject.SetActive(false);
        }
        unitsSelected.Clear();
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

    public void SetFormation()
    {
        for (int i = unitsSelected.Count - 1; i > 0; i--)
        {
            if (unitsSelected.Count % i == 0)
            {
                Debug.Log(i);
                var sqrt = Mathf.Sqrt(i); // karekökünü alarak en düşük sayıdaki birlik genişliğini elde ediyoruz.
                Debug.Log($"sqrt değeri {sqrt}");
                
                unitWidth = Mathf.CeilToInt(sqrt); // Birlik genişliğini bu sonuca eşitliyoruz.
                //Debug.Log($"unitWidth değeri {unitWidth}");
                
                var unitDepthSqrt = unitsSelected.Count / unitWidth; // Seçilen unitleri bulduğumuz genişliğe bölerek, birlik derinliğini hesaplıyoruz.
                //Debug.Log($"unitDepthSqrt değeri {unitDepthSqrt}");
                
                unitDepth = Mathf.RoundToInt(unitDepthSqrt);
                //Debug.Log($"unitDepth değeri {unitDepth}");
                
                var mod = unitsSelected.Count - (unitWidth * unitDepth); // Birlik formasyonunda arta kalan unitlerin sayısını hesaplıyoruz.
                //Debug.Log($"mod değeri {mod}");
                
                Debug.Log($"{unitWidth}x{unitDepth} boyutlu formasyon ve {mod} fazlalık.");
                break;
            }
        }
        
        //var middleOffset = new Vector3(unitWidth * 0.5f, 0, _unitDepth * 0.5f);
//
        //for (var x = 0; x < _unitWidth; x++) {
        //    for (var z = 0; z < _unitDepth; z++) {
        //        if (_hollow && x != 0 && x != _unitWidth - 1 && z != 0 && z != _unitDepth - 1) continue;
        //        var pos = new Vector3(x + (z % 2 == 0 ? 0 : _nthOffset), 0, z);
//
        //        pos -= middleOffset;
        //    }
        //}
        //
        //for (var i = 0; i < unitsSelected.Count; i++) {
        //    unitsSelected[i].transform.position = Vector3.MoveTowards(unitsSelected[i].transform.position, transform.position + _points[i], 4 * Time.deltaTime);
        //}
    }
}
