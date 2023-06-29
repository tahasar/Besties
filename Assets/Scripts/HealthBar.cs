using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{

    private Camera _cam;

    void Start(){
        _cam = Camera.main;
    }
    
    [SerializeField] private Image _healthbarSprite;
    // Start is called before the first frame update
    public void UpdateHealthBar(float maxHealth, float currentHealth){
        _healthbarSprite.fillAmount = currentHealth / maxHealth;
    }

    void Update(){
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
    }
}
