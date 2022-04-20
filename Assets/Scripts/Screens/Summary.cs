using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Text;
using System;

public class Summary : Screen
{
    public class Vote
    {
        public string ign;
        public int votes;

        public void GetVotes(int amount)
        {
            votes += amount;
            Debug.Log(ign + " is now on: " + votes + " votes.");
        }
    }
    public List<Vote> votes = new List<Vote>();
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
        UIHandler.Instance.DispatchPopUp("Import data", "Make sure everyone has submitted their votes and voting is done if the intend of this action is to calculate the final results. NOTE: Since the Google Drive solution is not yet ready, please download the google drive folder, place it on your desktop and enter it's name below.", true, true, OptionType.ImportVotes, "Results folder name..."); 
    }

    public void AnalyzeResultFile(string path, string fileName)
    {
        string textContents;

        try
        {
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                textContents = streamReader.ReadToEnd();
            }
        }
        catch (FileNotFoundException ex)
        {
            Dispatch(false);
            UIHandler.Instance.DispatchPopUp("Something went wrong..", "Could not read content. Please try again", false, false, OptionType.ImportData, "");
            Debug.LogError("file is null. Exception: " + ex);
            return;
        }

        try
        {
            CandidateData[] candData = JsonHelper.FromJson<CandidateData>(textContents);
            for (int i = 0; i < candData.Length; i++)
            {
                CountVote(candData[i]);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
            StartCoroutine(LoadError());
            return;
        }
    }

    private void CountVote(CandidateData data)
    {
        if (votes.Count == 0)
        {
            Debug.Log("added: " + data.ign);
            votes.Add(new Vote { ign = data.ign, votes = data.points });
            return;
        }

        foreach(var vote in votes)
        {
            if (vote.ign == data.ign)
                Debug.Log(data.ign + " already exists");
            else
            {
                Debug.Log("added: " + data.ign);
                AddVote(new Vote { ign = data.ign, votes = data.points });
            }
        }
    }

    private void AddVote(Vote vote) { votes.Add(vote); }

    private IEnumerator LoadError()
    {
        yield return new WaitForSeconds(0.25f);
        UIHandler.Instance.DispatchPopUp("Something went wrong..", "The voting file could not be read. Please check it's format and/or contents. File might be empty or tampered with post-export.", false, false, OptionType.Misc, "");
    }

    public void GetFinalScores()
    {
        foreach (var vote in votes)
            Debug.Log("Votes for user: " + vote.ign + " is: " + vote.votes);
    }
}