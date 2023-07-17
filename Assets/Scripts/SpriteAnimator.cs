using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] private float hoverHeight = 10f; // The amount of vertical movement when the UI element is hovered.
    [SerializeField] private float hoverSpeed = 1f; // The speed of the hover animation.

    private Vector3 originalPosition;
    private bool isHovering = false;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    private void Update()
    {
        // Calculate the new position for the hover animation.
        Vector3 targetPosition = originalPosition + Vector3.up * Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;
        transform.localPosition = targetPosition;
    }
}
