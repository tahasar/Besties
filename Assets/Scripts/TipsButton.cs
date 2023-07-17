using UnityEngine;
using UnityEngine.UI;

public class TipsButton : MonoBehaviour
{
    public GameObject tipsMessagePanel;

    private void Start()
    {
        tipsMessagePanel.SetActive(false);
    }

    public void OnTipsButtonClicked()
{

    if (!tipsMessagePanel.activeSelf)
    {
        tipsMessagePanel.SetActive(true);
    }
    else
    {
        tipsMessagePanel.SetActive(false);
    }
}


    public void OnCloseButtonClicked()
    {
        tipsMessagePanel.SetActive(false);
    }
}
