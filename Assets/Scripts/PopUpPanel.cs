using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

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
        switch(CurrentType)
        {
            case OptionType.ResetData:
                Application.Quit();
                break;

            case OptionType.ExportData:
                List<CandidateData> candidatesData = new List<CandidateData>();
                
                foreach (var approved in DataManager.Instance.ApprovedSeekers)
                {
                    CandidateData data = new CandidateData();
                    data.ign = approved.Key.IGN;
                    data.points = approved.Value;
                    candidatesData.Add(data);
                }

                foreach (var denied in DataManager.Instance.DeniedSeekers)
                {
                    CandidateData data = new CandidateData();
                    data.ign = denied.Key.IGN;
                    data.points = denied.Value;
                    candidatesData.Add(data);
                }

                string candidatesJson = JsonHelper.ToJson(candidatesData.ToArray(), true);
                string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path = deskPath + @"\fs_export.json";

                try
                {
                    File.WriteAllText(path, candidatesJson);
                }
                catch (Exception ex)
                {
                    Dispatch(false);
                    Debug.LogError("Something went wrong. Exception: " + ex);
                    UIHandler.Instance.DispatchPopUp("Error on export", "Something wen't wrong trying to export your data. Try again", false, false, OptionType.ExportData);
                    return;
                }
               
                break;
        }

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
                DataManager.Instance.SaveCurrentData();
                break;

            case OptionType.ResetData:
                DataManager.Instance.ResetData();
                try
                {
                    Dispatch(false);
                    UIHandler.Instance.DispatchPopUp("Restart required", "You have reset your tool's data. Please restart the tool in order for the reset to take effect", false, false, OptionType.ResetData);
                    return;
                } 
                catch (Exception ex) { Debug.LogError(ex); }
                break;
        }

        Dispatch(false);
    }

}