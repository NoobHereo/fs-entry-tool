using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum CandidateVoteType
{
    Approved,
    Denied
}

public class Candidates : Screen
{
    public TextMeshProUGUI noCandidates;
    public Button ApprovedButton, DeniedButton;
    public GameObject CandidatesPanel;

    public override void OnBackClick()
    {
        UIHandler.Instance.ShowMenu();
        Dispatch(false);
    }

    public void InitData()
    {
        ApprovedButton.onClick.AddListener(OnApprovedClick);
        DeniedButton.onClick.AddListener(OnDeniedClick);
        DeniedButton.interactable = true;
        ApprovedButton.interactable = false;

        if (DataManager.Instance.ApprovedSeekers == null || DataManager.Instance.ApprovedSeekers.Count <= 0)
        {
            noCandidates.gameObject.SetActive(true);
            return;
        }
        else
        {
            RemoveBoxes();
            foreach (var fs in DataManager.Instance.ApprovedSeekers)
            {
                GameObject nameBox = Instantiate(Resources.Load<GameObject>("Prefabs/CandidateBox"), CandidatesPanel.transform);
                nameBox.GetComponent<CandidateBox>().InitBox(fs.Key, fs.Value);
            }
        }
    }

    private void OnApprovedClick()
    {
        DeniedButton.interactable = true;
        ApprovedButton.interactable = false;
        if (DataManager.Instance.ApprovedSeekers == null || DataManager.Instance.ApprovedSeekers.Count <= 0)
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
        }
    }

    private void OnDeniedClick()
    {
        ApprovedButton.interactable = true;
        DeniedButton.interactable = false;
        if (DataManager.Instance.DeniedSeekers == null || DataManager.Instance.DeniedSeekers.Count <= 0)
        {
            RemoveBoxes();
            noCandidates.gameObject.SetActive(true);
            return;
        }
        else
        {
            noCandidates.gameObject.SetActive(false);
            RemoveBoxes();
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