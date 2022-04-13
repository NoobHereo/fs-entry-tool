using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InspectPanel : MonoBehaviour
{
    public TextMeshProUGUI TextContainer;
    public Button CloseButton;

    public void Dispatch(bool visible, string text)
    {
        transform.GetChild(0).gameObject.SetActive(visible);

        if (visible)
        {
            CloseButton.onClick.RemoveAllListeners();
            CloseButton.onClick.AddListener(OnCloseClick);
        }

        if (string.IsNullOrEmpty(text))
            TextContainer.text = "Nothing to see...";
        else
            TextContainer.text = text;
    }

    public void OnCloseClick()
    {
        Dispatch(false, "");
    }

    
}