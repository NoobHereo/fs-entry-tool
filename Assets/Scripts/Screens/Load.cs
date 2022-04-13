using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[Serializable]
public class FutureSeekerData
{
    public string IGN;
    public string why;
    public string what;
    public string your_message;
    public string programming_experience;
    public string artistic_feedback;
    public string public_feedback;
    public string shatters;
    public string statuseffect;
    public string gooddesign;
    public string agree;
}

public class Load : Screen
{
    public Dictionary<int, FutureSeekerData> FSEntries = new Dictionary<int, FutureSeekerData>();
    public TextMeshProUGUI ign, why, what, your_message, programming_exp, art_fb, public_fb, shatters, stateff, design, agree;
    public TextMeshProUGUI Title, NoEntries;
    public Button LeftButton, RightButton, CandidateButton, SearchButton, AnswerInspectButton;
    public GameObject EntryPanel;
    public Scrollbar Scrollbar;
    public TMP_InputField PointsField;
    public TMP_FontAsset RegularFont, CustomFont;
    public SearchPanel SearchPanel;
    public InspectPanel InspectPanel;

    public FutureSeekerData CurrentSeeker;
    private int currentSeekerId;
    private int totalEntries;
    private string inspectText = string.Empty;

    public override void OnBackClick()
    {
        UIHandler.Instance.ShowMenu();
        Dispatch(false);
    }

    public void InitData()
    {
        FSEntries = new Dictionary<int, FutureSeekerData>();

        if (DataManager.Instance.LOADED_JSON != null && !string.IsNullOrWhiteSpace(DataManager.Instance.LOADED_JSON))
        {
            string json = DataManager.Instance.LOADED_JSON.ToString();
            FutureSeekerData[] entryJSONS = JsonHelper.FromJson<FutureSeekerData>(json);
            if (entryJSONS == null || entryJSONS.Length <= 0)
            {
                DisplayNoEntry();
                return;
            }
            for (int i = 0; i < entryJSONS.Length; i++)
                FSEntries.Add(i, entryJSONS[i]);
        }
        else
        {
            DisplayNoEntry();
            return;
        }

        EntryPanel.SetActive(true);
        NoEntries.gameObject.SetActive(false);
        CandidateButton.interactable = true;
        AnswerInspectButton.gameObject.SetActive(false);

        if (currentSeekerId == 0)
            LeftButton.interactable = false;

        RightButton.interactable = true;
        Scrollbar.gameObject.SetActive(true);
        Title.gameObject.SetActive(true);

        CurrentSeeker = FSEntries[currentSeekerId];
        totalEntries = FSEntries.Count;

        LeftButton.onClick.RemoveAllListeners();
        RightButton.onClick.RemoveAllListeners();
        CandidateButton.onClick.RemoveAllListeners();
        SearchButton.onClick.RemoveAllListeners();
        AnswerInspectButton.onClick.RemoveAllListeners();

        LeftButton.onClick.AddListener(OnLeftClick);
        RightButton.onClick.AddListener(OnRightClick);
        CandidateButton.onClick.AddListener(OnCandidateClick);
        SearchButton.onClick.AddListener(OnSearchClick);
        AnswerInspectButton.onClick.AddListener(OnInspectClick);
        UpdateEntryContent();
    }

    public void UpdateEntryContent()
    {
        CurrentSeeker = FSEntries[currentSeekerId];
        totalEntries = FSEntries.Count;
        Title.text = "Currently viewing entry: " + (currentSeekerId + 1) + "/" + totalEntries + " for applicant: " + FSEntries[currentSeekerId].IGN + "(IGN)";
        EntryPanel.gameObject.GetComponent<Outline>().effectColor = Color.black;
        bool approved = false;
        bool denied = false;

        if (currentSeekerId == 0)
            LeftButton.interactable = false;

        foreach (var candidate in DataManager.Instance.Candidates)
        {
            if (candidate.Key.IGN == CurrentSeeker.IGN)
            {
                EntryPanel.gameObject.GetComponent<Outline>().effectColor = Color.green;
                CandidateButton.interactable = false;
                approved = true;
                break;
            }
        }        

        if (!approved && !denied)
            CandidateButton.interactable = true;
        

        ign.text = CurrentSeeker.IGN;
        why.text = CurrentSeeker.why;
        what.text = CurrentSeeker.what;
        your_message.text = CurrentSeeker.your_message;
        programming_exp.text = CurrentSeeker.programming_experience;
        art_fb.text = CurrentSeeker.artistic_feedback;
        public_fb.text = CurrentSeeker.public_feedback;
        shatters.text = CurrentSeeker.shatters;
        stateff.text = CurrentSeeker.statuseffect;
        design.text = CurrentSeeker.shatters;
        agree.text = CurrentSeeker.agree;
    }

    private void OnLeftClick()
    {
        currentSeekerId--;

        if (currentSeekerId == 0)     
            LeftButton.interactable = false;

        if (currentSeekerId == (totalEntries - 2))
            RightButton.interactable = true;

        SetAnswerInspectButton(false, string.Empty);
        UpdateEntryContent();
    }

    private void OnRightClick()
    {
        currentSeekerId++;

        if (currentSeekerId > 0)
            LeftButton.interactable = true;

        if (currentSeekerId == (totalEntries - 1))
            RightButton.interactable = false;

        SetAnswerInspectButton(false, string.Empty);
        UpdateEntryContent();
    }

    public void DisplayNoEntry()
    {
        EntryPanel.SetActive(false);
        NoEntries.gameObject.SetActive(true);
        CandidateButton.interactable = false;
        LeftButton.interactable = false;
        RightButton.interactable = false;
        Scrollbar.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);
    }

    private void OnCandidateClick()
    {
        SetAnswerInspectButton(false, string.Empty);
        CandidateButton.interactable = false;

        int points;
        if (string.IsNullOrEmpty(PointsField.text))
            points = 0;
        else
            points = int.Parse(PointsField.text);

        if (points < 0 || points > 8)
        {
            UIHandler.Instance.DispatchPopUp("Invalid points", "You entered an invalid amount of points. You can give a min. score of 0 and maximum score of 8. Leave it blank to delegate 0 points.", false, false, OptionType.Misc, "");
            CandidateButton.interactable = true;
            return;
        }


        DataManager.Instance.AddSeeker(FSEntries[currentSeekerId], points);
        DataManager.Instance.SaveCurrentData();
        OnRightClick();
    }

    private void OnSearchClick()
    {
        SearchPanel.Dispatch(true);
    }

    public void SetFont(bool customFont)
    {
        TMP_FontAsset font = customFont ? CustomFont : RegularFont;
        float size = customFont ? 44 : 40;
        
        ign.font = font;
        ign.fontSizeMax = size;

        why.font = font;
        why.fontSizeMax = size;

        what.font = font;
        what.fontSizeMax = size;

        your_message.font = font;
        your_message.fontSizeMax = size;

        programming_exp.font = font;
        programming_exp.fontSizeMax = size;

        art_fb.font = font;
        art_fb.fontSizeMax = size;

        public_fb.font = font;
        public_fb.fontSizeMax = size;

        shatters.font = font;
        shatters.fontSizeMax = size;

        stateff.font = font;
        stateff.fontSizeMax = size;

        design.font = font;
        design.fontSizeMax = size;

        agree.font = font;
        agree.fontSizeMax = size;

    }

    public void JumpToEntry(string ign)
    {
        foreach(var fs in FSEntries)
        {
            if (fs.Value.IGN == ign)
            {
                currentSeekerId = fs.Key;
                UpdateEntryContent();
            }
        }
    }

    public void SetAnswerInspectButton(bool visible, string answer)
    {
        AnswerInspectButton.gameObject.SetActive(visible);
        inspectText = answer;
    }

    public void OnInspectClick()
    {
        InspectPanel.Dispatch(true, inspectText);
    }
}