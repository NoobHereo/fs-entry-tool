using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

public enum OptionType
{
    ImportData,
    ResetData,
    ExportData,
    ImportVotes,
    CandEdit,
    Misc
}

public struct GData
{
    public string ign;
    public int points;
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

    public void InitComponents(string title, string description, bool inputFIeld, bool dualButtons, OptionType type, string inputText)
    {
        Title.text = title;
        Description.text = description;
        CurrentType = type;
        ConfirmButton.onClick.AddListener(OnConfirmClick);
        CancelButton.onClick.AddListener(OnCancelClick);
        OKButton.onClick.AddListener(OnOKClick);
        if (inputFIeld)
        {
            DataField.text = "";
            DataField.transform.GetChild(0).Find("Placeholder").GetComponent<TextMeshProUGUI>().text = inputText;
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
                
                foreach (var candidate in DataManager.Instance.Candidates)
                {
                    CandidateData data = new CandidateData();
                    data.ign = candidate.Key.IGN;
                    data.points = candidate.Key.Points;
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
                    UIHandler.Instance.DispatchPopUp("Error on export", "Something wen't wrong trying to export your data. Try again", false, false, OptionType.ExportData, "");
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
                    UIHandler.Instance.DispatchPopUp("Error: File not found", "You tried to import a file but the file was not found. Please try again", false, false, OptionType.ImportData, "");
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
                    UIHandler.Instance.DispatchPopUp("Restart required", "You have reset your tool's data. Please restart the tool in order for the reset to take effect", false, false, OptionType.ResetData, "");
                    return;
                } 
                catch (Exception ex) { Debug.LogError(ex); }
                break;

            case OptionType.CandEdit:
                if (string.IsNullOrEmpty(DataField.text))
                    return;

                UIHandler.Instance.Candidates.UpdateCurrentCandidate(int.Parse(DataField.text));
                break;

            case OptionType.ImportVotes:
                ProcessFolder(DataField.text);
                break;
        }

        Dispatch(false);
    }

    private void ProcessFolder(string folderName)
    {
        string deskPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string path = deskPath + @"\" + folderName;

        var info = new DirectoryInfo(path);
        var fileInfo = info.GetFiles();

        if (fileInfo.Length == 0)
        {
            Debug.Log("the folder is empty");
            return;
        }

        foreach (var file in fileInfo)
            UIHandler.Instance.Summary.AnalyzeResultFile(path + @"\" + file.Name, file.Name);

        UIHandler.Instance.Summary.GetFinalScores();
    }

    #region Deprecated code
    /////// READ ME ///////

    // This part has been commented out as it's yet to be done using the Google Drive API instead of Unity's Web Request package.
    // I'm not a big fan of regions either, but I would rather not look at this big box of green. Thank you for understanding :)

    ///////////////////////

    //private void ProcessLink(string url)
    //{
    //    Debug.Log("base url: " + url);
    //    string fileId = url.Substring(39, 33);
    //    string baseUrl = "https://drive.google.com/uc?export=download&id=";

    //    Debug.Log(baseUrl + fileId);
    //    StartCoroutine(GetData(baseUrl + fileId));
    //}

    //private IEnumerator GetData(string url)
    //{
    //    UnityWebRequest request = UnityWebRequest.Get(url);
    //    yield return request.SendWebRequest();

    //    if (request.result == UnityWebRequest.Result.ConnectionError)
    //    {
    //        Debug.LogError("something went wrong...");
    //    }
    //    else
    //    {
    //        Debug.Log("Downloaded: " + request.downloadHandler.text);
    //        GData data = JsonUtility.FromJson<GData>(request.downloadHandler.text);
    //        Debug.Log("data: " + data.ToString());
    //    }

    //    request.Dispose();
    //}
    #endregion
}