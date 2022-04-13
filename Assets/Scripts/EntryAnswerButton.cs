using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryAnswerButton : MonoBehaviour
{
    private Button btn;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        UIHandler.Instance.Load.SetAnswerInspectButton(true, gameObject.GetComponent<TextMeshProUGUI>().text);
    }
}