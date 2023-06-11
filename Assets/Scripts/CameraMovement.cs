using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float speed = 0.0035f;
    private float zoomSpeed = 10f;
    private float rotationSpeed = 0.1f;

    private float maxHeight = 60f;
    private float minHeight = 25f;

    private Vector2 p1;
    private Vector2 p2;

    public GameObject camera;
    private Transform cameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = camera.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 0.01f;
            zoomSpeed = 10f;
        }
        else
        {
            speed = 0.0035f;
            zoomSpeed = 5f;
        }
            
        float hsp = transform.position.y * speed * Input.GetAxis("Horizontal");
        float vsp = transform.position.y * speed * Input.GetAxis("Vertical");
        float scrollSp = Mathf.Log(transform.position.y) * -zoomSpeed * Input.GetAxis("Mouse ScrollWheel");

        if ((transform.localPosition.y >= maxHeight) && (scrollSp > 0))
        {
            scrollSp = 0;
            transform.localPosition = new Vector3(transform.position.x, 60, transform.position.z);
        }
        else if ((transform.localPosition.y <= minHeight) && (scrollSp < 0))
        {
            scrollSp = 0;
            transform.localPosition = new Vector3(transform.position.x, 25, transform.position.z);
        }
        
        Vector3 verticalMove = new Vector3(0, scrollSp, 0);
        Vector3 lateralMove = hsp * transform.right;
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= vsp;

        Vector3 move = verticalMove + lateralMove + forwardMove;

        transform.position += move;
        
        getCameraRotation();
        
        Debug.Log(transform.localEulerAngles);
    }

    void getCameraRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            p1 = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            p2 = Input.mousePosition;

            float dx = (p2 - p1).x * rotationSpeed;
            float dy = (p2 - p1).y * rotationSpeed;
            
            transform.rotation *= Quaternion.Euler(new Vector3(0,dx,0));
            cameraTransform.rotation *= Quaternion.Euler(new Vector3(-dy,0,0));

            if (cameraTransform.localEulerAngles.x > 60)
            {
                cameraTransform.rotation = Quaternion.Euler(new Vector3(60, 0, 0));
                Debug.Log("yüksek");
            }
            
            if (cameraTransform.localEulerAngles.x < 40)
            {
                cameraTransform.rotation = Quaternion.Euler(new Vector3(40, 0, 0));
                Debug.Log("alçak");
            }

            p1 = p2;
        }
    }
}
