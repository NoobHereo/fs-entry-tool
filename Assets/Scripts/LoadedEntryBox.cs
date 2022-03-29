using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadedEntryBox : MonoBehaviour
{
    private Button btn;
    public TextMeshProUGUI BoxText;

    public void InitBox(FutureSeekerData fs, int id)
    {
        BoxText.text = fs.IGN;
        btn = GetComponent<Button>();
    }
}