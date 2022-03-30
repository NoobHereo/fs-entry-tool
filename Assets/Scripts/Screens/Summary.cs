using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Summary : Screen
{
    public Button ImportButton;

    public override void OnBackClick()
    {
        UIHandler.Instance.ShowMenu();
        Dispatch(false);
    }

    public void Init()
    {
        ImportButton.onClick.RemoveAllListeners();
        ImportButton.onClick.AddListener(OnImportClick);
    }

    private void OnImportClick()
    {
        UIHandler.Instance.DispatchPopUp("Import data", "Make sure everyone has submitted their votes and voting is done if the intend of this action is to calculate the final results.", true, true, OptionType.ImportVotes, "Sample text"); 
    }
}