using UnityEngine;
using UnityEngine.UI;

public class Screen : MonoBehaviour
{
    public Button BackButton;

    public virtual void Start() 
    { 
        if (BackButton != null)
            BackButton.onClick.AddListener(OnBackClick);
    }

    /// <summary>
    /// OnClick event for all screens back buttons
    /// </summary>
    public virtual void OnBackClick() { }

    /// <summary>
    /// Dispatch function that toggles a screen's visiblity. NOTE: The screen object's child is the 
    /// parent of all the screen's content
    /// </summary>
    /// <param name="visible"></param>
    public virtual void Dispatch(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);
    }

    public void SetTheme(bool isMenu = false)
    {
        Image backfill = transform.GetChild(0).GetComponent<Image>();
        int alpha = isMenu ? 0 : 85;

        if (UIHandler.Instance.AppTheme == ScreenThemes.Basic)
            backfill.color = new Color32(65, 65, 65, 255);

        else if (UIHandler.Instance.AppTheme == ScreenThemes.Exalt)
            backfill.color = new Color32(0, 0, 0, (byte)alpha);
    }
}