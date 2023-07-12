using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class MenuButton : MonoBehaviour
{
    public AudioClip clickSound;
    public Button button;
    public Image buttonImage;
    public Sprite idleSprite;
    public Sprite hoverSprite;
    public Sprite clickSprite;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // Perform button click action here
    }

    public void OnPointerEnter()
    {
        buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit()
    {
        buttonImage.sprite = idleSprite;
    }

    public void OnPointerDown()
    {
        buttonImage.sprite = clickSprite;
    }

    public void OnPointerUp()
    {
        buttonImage.sprite = hoverSprite;
    }
}
