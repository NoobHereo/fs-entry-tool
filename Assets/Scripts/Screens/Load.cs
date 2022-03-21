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
    public Button LeftButton, RightButton, ApproveButton, DenyButton;
    public GameObject EntryPanel;
    public Scrollbar Scrollbar;
    public TMP_InputField PointsField;
    public TMP_FontAsset RegularFont, CustomFont;

    public FutureSeekerData CurrentSeeker;
    private int currentSeekerId;
    private int totalEntries;

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
        ApproveButton.interactable = true;
        DenyButton.interactable = true;

        if (currentSeekerId == 0)
            LeftButton.interactable = false;

        RightButton.interactable = true;
        Scrollbar.gameObject.SetActive(true);
        Title.gameObject.SetActive(true);

        // currentSeekerId = 0;
        CurrentSeeker = FSEntries[currentSeekerId];
        totalEntries = FSEntries.Count;

        LeftButton.onClick.RemoveAllListeners();
        RightButton.onClick.RemoveAllListeners();
        ApproveButton.onClick.RemoveAllListeners();
        DenyButton.onClick.RemoveAllListeners();

        LeftButton.onClick.AddListener(OnLeftClick);
        RightButton.onClick.AddListener(OnRightClick);
        ApproveButton.onClick.AddListener(OnApproveClick);
        DenyButton.onClick.AddListener(OnDenyClick);
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
        
        foreach(var fs in DataManager.Instance.ApprovedSeekers)
        {
            if (fs.Key.IGN == CurrentSeeker.IGN)
            {
                EntryPanel.gameObject.GetComponent<Outline>().effectColor = Color.green;
                ApproveButton.interactable = false;
                DenyButton.interactable = false;
                approved = true;
                break;
            }
        }
        foreach(var fs in DataManager.Instance.DeniedSeekers)
        {
            if (fs.Key.IGN == CurrentSeeker.IGN)
            {
                EntryPanel.gameObject.GetComponent<Outline>().effectColor = Color.red;
                ApproveButton.interactable = false;
                DenyButton.interactable = false;
                denied = true;
                break;
            }
        }

        if (!approved && !denied)
        {
            ApproveButton.interactable = true;
            DenyButton.interactable = true;
        }

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

        UpdateEntryContent();
    }

    private void OnRightClick()
    {
        Debug.Log("click");
        currentSeekerId++;

        if (currentSeekerId > 0)
            LeftButton.interactable = true;

        if (currentSeekerId == (totalEntries - 1))
            RightButton.interactable = false;

        UpdateEntryContent();
    }

    public void DisplayNoEntry()
    {
        EntryPanel.SetActive(false);
        NoEntries.gameObject.SetActive(true);
        ApproveButton.interactable = false;
        DenyButton.interactable = false;
        LeftButton.interactable = false;
        RightButton.interactable = false;
        Scrollbar.gameObject.SetActive(false);
        Title.gameObject.SetActive(false);
    }

    private void OnApproveClick()
    {
        ApproveButton.interactable = false;
        DenyButton.interactable = false;

        int points;
        if (string.IsNullOrEmpty(PointsField.text))
            points = 0;
        else
            points = int.Parse(PointsField.text);

        DataManager.Instance.AddSeeker(FSEntries[currentSeekerId], CandidateVoteType.Approved, points);
        DataManager.Instance.SaveCurrentData();
        OnRightClick();
    }

    private void OnDenyClick()
    {
        ApproveButton.interactable = false;
        DenyButton.interactable = false;

        int points;
        if (string.IsNullOrEmpty(PointsField.text))
            points = 0;
        else
            points = int.Parse(PointsField.text);

        DataManager.Instance.AddSeeker(FSEntries[currentSeekerId], CandidateVoteType.Denied, points);
        DataManager.Instance.SaveCurrentData();
        OnRightClick();
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
}