using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Health health = null;
    [SerializeField] private GameObject healthBarParent = null;
    [SerializeField] private Image healthBarImage = null;

    private Camera _cam;
    
    private void Awake()
    {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDestroy()
    {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    private void Start()
    {
        _cam = Camera.main;
    }

    private void LateUpdate()
    {
        // Rotate the health bar to face the camera
        if (healthBarParent.activeSelf)
            healthBarParent.transform.LookAt(transform.position + _cam.transform.rotation * Vector3.forward,
                _cam.transform.rotation * Vector3.up);
    }

    public void OnMouseEnter()
    {
        healthBarParent.SetActive(true);
    }

    public void OnMouseExit()
    {
        healthBarParent.SetActive(false);
    }

    private void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}
