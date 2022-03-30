using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadedEntryBox : MonoBehaviour
{
    private Button btn;
    public TextMeshProUGUI BoxText;
    private SearchPanel search;
    private string ign;

    public void InitBox(FutureSeekerData fs, int id)
    {
        ign = fs.IGN;
        BoxText.text = ign;
        btn = GetComponent<Button>();
        search = transform.parent.parent.parent.parent.GetComponent<SearchPanel>(); // B-)
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        UIHandler.Instance.Load.JumpToEntry(ign);
        search.OnCloseClick();
    }
}