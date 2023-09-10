using UnityEngine;

namespace Cameras
{
    public class CameraMovement : MonoBehaviour
    {
        public static CameraMovement Instance;
        public float normalSpeed;
        public float fastSpeed;
        public float movementSpeed = 0.0035f;
        public float movementTime;
        public float rotationAmount;
        public Vector3 zoomAmount;

        private Vector3 _newPosition;
        private Quaternion _newRotation;
        private Vector3 _camZoom;

        private Vector3 _dragStartPosition;
        private Vector3 _dragCurrentPosition;
        private Vector3 _rotateStartPosition;
        private Vector3 _rotateCurrentPosition;

        private const int LowestZoom = 5;
        private const int HighestZoom = 45;

        private const int MinBorderZ = -100;
        private const int MaxBorderZ = 100;
        private const int MinBorderX = -100;
        private const int MaxBorderX = 100;

        public Transform cameraTransform;
    
        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
        
            var transform1 = transform;
            _newPosition = transform1.position;
            _newRotation = transform1.rotation;
            _camZoom = cameraTransform.position;


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
                _camZoom += zoomAmount * (Input.mouseScrollDelta.y * movementTime);
                _camZoom.y = Mathf.Clamp(_camZoom.y, LowestZoom, HighestZoom);
            
            }
        
            if (Input.GetMouseButtonDown(2))
            {
                if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);
                
                    if (Camera.main != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        float entry;

                        if (plane.Raycast(ray, out entry))
                        {
                            _dragStartPosition = ray.GetPoint(entry);
                        }
                    }
                }
                else
                {
                    _rotateStartPosition = Input.mousePosition;
                }

            }

            if (Input.GetMouseButton(2))
            {
                if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);
                
                    if (Camera.main != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        float entry;

                        if (plane.Raycast(ray, out entry))
                        {
                            _dragCurrentPosition = ray.GetPoint(entry);

                            _newPosition = transform.position + _dragStartPosition - _dragCurrentPosition;
                            _newPosition.x = Mathf.Clamp(_newPosition.x, MinBorderX, MaxBorderX);
                            _newPosition.z = Mathf.Clamp(_newPosition.z, MinBorderZ, MaxBorderZ);
                        }
                    }
                }
                else
                {
                    _rotateCurrentPosition = Input.mousePosition;

                    Vector3 difference = _rotateStartPosition - _rotateCurrentPosition;

                    _rotateStartPosition = _rotateCurrentPosition;
            
                    _newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
                    _newPosition.x = Mathf.Clamp(_newPosition.x, MinBorderX, MaxBorderX);
                    _newPosition.z = Mathf.Clamp(_newPosition.z, MinBorderZ, MaxBorderZ);
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
                _newPosition += (transform.forward * movementSpeed);
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _newPosition += (transform.forward * -movementSpeed);
            }
        
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _newPosition += (transform.right * movementSpeed);
            }
        
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _newPosition += (transform.right * -movementSpeed);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                _newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if (Input.GetKey(KeyCode.E))
            {
                _newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }
        
            if (Input.GetKey(KeyCode.R))
            {
                _camZoom += zoomAmount;
                _camZoom.y = Mathf.Clamp(_camZoom.y, LowestZoom, HighestZoom);
            }
            if (Input.GetKey(KeyCode.F))
            {
                _camZoom += -zoomAmount;
                _camZoom.y = Mathf.Clamp(_camZoom.y, LowestZoom, HighestZoom);
            }

            Transform transform1;
            (transform1 = transform).position = Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime);
            transform.rotation = Quaternion.Lerp(transform1.rotation, _newRotation, Time.deltaTime * movementTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, _camZoom, Time.deltaTime * movementTime);
        
            _newPosition.x = Mathf.Clamp(_newPosition.x, MinBorderX, MaxBorderX);
            _newPosition.z = Mathf.Clamp(_newPosition.z, MinBorderZ, MaxBorderZ);
        }
    }
}
