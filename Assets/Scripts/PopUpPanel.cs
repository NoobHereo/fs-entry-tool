using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Text;

public enum OptionType
{
    ImportData,
    ResetData,
    ExportData
}

public class PopUpPanel : MonoBehaviour
{
    public TextMeshProUGUI Title, Description;
    public TMP_InputField DataField;
    public Button ConfirmButton, CancelButton, OKButton;
    public OptionType CurrentType;

    public void Dispatch(bool visible)
    {
        transform.GetChild(0).gameObject.SetActive(visible);

        if (!visible)
        {
            DataField.gameObject.SetActive(false);
            ConfirmButton.gameObject.SetActive(false);
            CancelButton.gameObject.SetActive(false);
            OKButton.gameObject.SetActive(false);
        }
    }

    public void InitComponents(string title, string description, bool inputFIeld, bool dualButtons, OptionType type)
    {
        Title.text = title;
        Description.text = description;
        CurrentType = type;
        ConfirmButton.onClick.AddListener(OnConfirmClick);
        CancelButton.onClick.AddListener(OnCancelClick);
        OKButton.onClick.AddListener(OnOKClick);
        if (inputFIeld)
        {
            DataField.gameObject.SetActive(true);
        }
        else
        {
            DataField.gameObject.SetActive(false);
        }

        if (dualButtons)
        {
            ConfirmButton.gameObject.SetActive(true);
            CancelButton.gameObject.SetActive(true);
            OKButton.gameObject.SetActive(false);
        }
        else
        {
            OKButton.gameObject.SetActive(true);
            ConfirmButton.gameObject.SetActive(false);
            CancelButton.gameObject.SetActive(false);
        }
    }

    private void OnCancelClick()
    {
        Dispatch(false);
    }

    private void OnOKClick()
    {
        Dispatch(false);
    }

    private void OnConfirmClick()
    {
        switch(CurrentType)
        {
            case OptionType.ImportData:
                TextAsset temp = Resources.Load<TextAsset>("Data/jsonTemplate");
                string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path = deskPath + @"\" + DataField.text + ".json";

                string textContents;

                try
                {
                    var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                    {
                        textContents = streamReader.ReadToEnd();
                    }
                }
                catch(FileNotFoundException ex)
                {
                    Dispatch(false);
                    UIHandler.Instance.DispatchPopUp("Error: File not found", "You tried to import a file but the file was not found. Please try again", false, false, OptionType.ImportData);
                    Debug.LogError("file is null. Exception: " + ex);
                    return;
                }                

                string json = temp.text + textContents + "}";
                DataManager.Instance.LoadJSON(json);
                break;

            case OptionType.ResetData:
                Debug.Log("reset");
                break;
        }

        Dispatch(false);
    }

}