using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Cameras
{
    public class Minimap : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
    
        [SerializeField] private RectTransform minimapRect = null;
        [SerializeField] private float mapScale = 25f;
        [SerializeField] private float offset = -6f;
    
        private GameObject _playerCameraTransform;

        private void Update()
        {
            if (_playerCameraTransform != null) { return; }
        
            if (NetworkClient.connection?.identity == null) { return; }
        
            _playerCameraTransform = NetworkClient.connection.identity.
                GetComponent<RTSPlayer>().GetCameraTransform();
        }

        private void MoveCamera()
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
        
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    minimapRect, 
                    mousePos, 
                    null, 
                    out Vector2 localPoint)) 
            { return; }

            var rect = minimapRect.rect;
            Vector2 lerp = new Vector2(
                (localPoint.x - rect.x) / rect.width, 
                (localPoint.y - rect.y) / rect.height);
        
            Vector3 newCameraPos = new Vector3(
                Mathf.Lerp(-mapScale, mapScale, lerp.x), 
                _playerCameraTransform.transform.position.y,
                Mathf.Lerp(-mapScale, mapScale, lerp.y));
        
            _playerCameraTransform.transform.position = newCameraPos + new Vector3(0,0,offset);
        }

        public void OnPointerDown(PointerEventData eventData)
        { 
            MoveCamera();
        }

        public void OnDrag(PointerEventData eventData)
        {
            MoveCamera();
        }
    }
}
