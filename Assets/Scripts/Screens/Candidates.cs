using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum CandidateVoteType
{
    Approved,
    Denied
}

[Serializable]
public class CandidateData
{
    public string ign;
    public int points;
}

public class Candidates : Screen
{
    public TextMeshProUGUI noCandidates;
    public GameObject CandidatesPanel;

    public override void OnBackClick()
    {
        UIHandler.Instance.ShowMenu();
        Dispatch(false);
    }

    public void InitData()
    {
        if (DataManager.Instance.ApprovedSeekers == null && DataManager.Instance.DeniedSeekers == null || DataManager.Instance.ApprovedSeekers.Count <= 0 && DataManager.Instance.DeniedSeekers.Count <= 0)
        {
            noCandidates.gameObject.SetActive(true);
            return;
        }
        else
        {
            noCandidates.gameObject.SetActive(false);
            RemoveBoxes();
            foreach (var fs in DataManager.Instance.ApprovedSeekers)
            {
                GameObject nameBox = Instantiate(Resources.Load<GameObject>("Prefabs/CandidateBox"), CandidatesPanel.transform);
                nameBox.GetComponent<CandidateBox>().InitBox(fs.Key, fs.Value);
            }
            foreach(var fs in DataManager.Instance.DeniedSeekers)
            {
                GameObject nameBox = Instantiate(Resources.Load<GameObject>("Prefabs/CandidateBox"), CandidatesPanel.transform);
                nameBox.GetComponent<CandidateBox>().InitBox(fs.Key, fs.Value);
            }
        }
    }

    public void RemoveBoxes()
    {
        for (int i = 0; i < CandidatesPanel.transform.childCount; i++)
        {
            Destroy(CandidatesPanel.transform.GetChild(i).gameObject);
        }
    }

}