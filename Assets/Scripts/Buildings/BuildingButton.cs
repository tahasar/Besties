﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Building building = null;
    [SerializeField] private Image iconImage = null;
    [SerializeField] private TMP_Text priceText = null;
    [SerializeField] private LayerMask floorMask = new LayerMask();

    private Camera _mainCamera;
    private SphereCollider _buildingCollider;
    private RTSPlayer _player;
    private GameObject _buildingPreviewInstance;
    private Renderer _buildingRendererInstance;

    private void Start()
    {
        _mainCamera = Camera.main;

        iconImage.sprite = building.GetIcon();
        priceText.text = building.GetPrice().ToString();

        _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

        _buildingCollider = building.GetComponent<SphereCollider>();
    }

    private void Update()
    {            
        if (_buildingPreviewInstance == null) { return; }

        UpdateBuildingPreview();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        if (_player.GetResources() < building.GetPrice()) { return; }

        _buildingPreviewInstance = Instantiate(building.GetBuildingPreview());
        _buildingRendererInstance = _buildingPreviewInstance.GetComponentInChildren<Renderer>();

        _buildingPreviewInstance.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_buildingPreviewInstance == null) { return; }

        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
        {
            if(_player.CanPlaceBuilding(_buildingCollider, hit.point) && 
               CheckForMapBorders(hit))
            {
                _player.CmdTryPlaceBuilding(building.GetId(), hit.point);
            }
        }
        
        Destroy(_buildingPreviewInstance);
    }

    private void UpdateBuildingPreview()
    {
        var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        _buildingPreviewInstance.transform.position = hit.point;

        if (!_buildingPreviewInstance.activeSelf)
        {
            _buildingPreviewInstance.SetActive(true);
        }

        Color color = _player.CanPlaceBuilding(_buildingCollider, hit.point) && 
                      CheckForMapBorders(hit) ? 
            Color.green : Color.red;

        _buildingRendererInstance.material.color = color;
    }
    
    private bool CheckForMapBorders(RaycastHit hit)
    {
        if (hit.collider.gameObject.CompareTag("Ground"))
        {
            return true;
        }else
            return false;
    }
}
