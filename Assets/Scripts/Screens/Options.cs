using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Options : Screen
{
    public Button ImportButton, ResetButton, ExportButton, CustomFontsButton;
    [SerializeField] private bool customFonts = true;

    public override void OnBackClick()
    {
        UIHandler.Instance.ShowMenu();
        Dispatch(false);
    }

    public void Init()
    {
        ImportButton.onClick.RemoveAllListeners();
        ResetButton.onClick.RemoveAllListeners();
        ExportButton.onClick.RemoveAllListeners();
        CustomFontsButton.onClick.RemoveAllListeners();

        ImportButton.onClick.AddListener(OnImportClick);
        ResetButton.onClick.AddListener(OnResetClick);
        ExportButton.onClick.AddListener(OnExportClick);
        CustomFontsButton.onClick.AddListener(OnCustomFontsClick);
    }

    private void OnImportClick()
    {
        UIHandler.Instance.DispatchPopUp("Import data", "Make sure the JSON file is on your desktop. Type the name of the file in the field below. Example: 'future_seekers'. Do not include .JSON. NOTE: This will override any current data."/*"Copy the JSON data as regular text and paste it in the datafield here. Press the import button to import the data to the tool. NOTE: This will override any current data"*/, true, true, OptionType.ImportData, "Enter JSON file name...");
    }

    private void OnResetClick()
    {
        UIHandler.Instance.DispatchPopUp("Reset data", "This will reset your data and all your work will be permanently gone. If you want to make a backup press cancel and export your data before performing this action", false, true, OptionType.ResetData, "");
    }

    private void OnExportClick()
    {
        UIHandler.Instance.DispatchPopUp("Export data", "Data has been succesfully exported to your desktop", false, false, OptionType.ExportData, "");
    }

    private void OnCustomFontsClick()
    {
        customFonts = !customFonts;
        string text = customFonts ? "Custom font: On" : "Custom font: Off";
        CustomFontsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

        UIHandler.Instance.UpdateFont(customFonts);
    }

}