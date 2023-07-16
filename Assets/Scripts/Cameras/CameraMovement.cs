using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    public float normalSpeed;
    public float fastSpeed;
    public float movementSpeed = 0.0035f;
    public float movementTime;
    public float rotationAmount;
    public Vector3 zoomAmount;

    private Vector3 newPosition;
    private Quaternion newRotation;
    private Vector3 camZoom;

    private Vector3 dragStartPosition;
    private Vector3 dragCurrentPosition;
    private Vector3 rotateStartPosition;
    private Vector3 rotateCurrentPosition;
    
    private int lowestZoom = 5;
    private int highestZoom = 45;

    private int minBorderZ = -100;
    private int maxBorderZ = 100;
    private int minBorderX = -100;
    private int maxBorderX = 100;

    public Transform cameraTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        newPosition = transform.position;
        newRotation = transform.rotation;
        camZoom = cameraTransform.position;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleMouseInput();
        HandleMovementInput();
        
    }

    void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            camZoom += zoomAmount * (Input.mouseScrollDelta.y * movementTime);
            camZoom.y = Mathf.Clamp(camZoom.y, lowestZoom, highestZoom);
            
        }
        
        if (Input.GetMouseButtonDown(2))
        {
            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            else
            {
                rotateStartPosition = Input.mousePosition;
            }

        }

        if (Input.GetMouseButton(2))
        {
            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    newPosition.x = Mathf.Clamp(newPosition.x, minBorderX, maxBorderX);
                    newPosition.z = Mathf.Clamp(newPosition.z, minBorderZ, maxBorderZ);
                }
            }
            else
            {
                rotateCurrentPosition = Input.mousePosition;

                Vector3 difference = rotateStartPosition - rotateCurrentPosition;

                rotateStartPosition = rotateCurrentPosition;
            
                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
                newPosition.x = Mathf.Clamp(newPosition.x, minBorderX, maxBorderX);
                newPosition.z = Mathf.Clamp(newPosition.z, minBorderZ, maxBorderZ);
            }
        }
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        
        if (Input.GetKey(KeyCode.R))
        {
            camZoom += zoomAmount;
            camZoom.y = Mathf.Clamp(camZoom.y, lowestZoom, highestZoom);
        }
        if (Input.GetKey(KeyCode.F))
        {
            camZoom += -zoomAmount;
            camZoom.y = Mathf.Clamp(camZoom.y, lowestZoom, highestZoom);
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, camZoom, Time.deltaTime * movementTime);
        
        newPosition.x = Mathf.Clamp(newPosition.x, minBorderX, maxBorderX);
        newPosition.z = Mathf.Clamp(newPosition.z, minBorderZ, maxBorderZ);
    }
}
