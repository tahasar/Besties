using Mirror;
using TMPro;
using UnityEngine;

namespace Resources
{
    public class ResourcesDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text resourcesText = null;

        private RTSPlayer _player;

        private void Start()
        {
            _player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();

            ClientHandleResourcesUpdated(_player.GetResources());

            _player.ClientOnResourcesUpdated += ClientHandleResourcesUpdated;
        }

        private void OnDestroy()
        {
            _player.ClientOnResourcesUpdated -= ClientHandleResourcesUpdated;
        }

        private void ClientHandleResourcesUpdated(int resources)
        {
            resourcesText.text = $"Resources: {resources}";
        }
    }
}