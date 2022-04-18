using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    public CandidateBox PreviousBox, CurrentBox;
    private bool boxSelected = false;
    public Button EditButton, UndoButton, CompareButton;

    public override void OnBackClick()
    {
        UIHandler.Instance.ShowMenu();
        Dispatch(false);
    }

    public void InitData()
    {
        if (DataManager.Instance.Candidates == null || DataManager.Instance.Candidates.Count <= 0)
        {
            noCandidates.gameObject.SetActive(true);
            return;
        }
        else
        {
            noCandidates.gameObject.SetActive(false);
            RemoveBoxes();
            foreach(var candidate in DataManager.Instance.Candidates)
            {
                GameObject nameBox = Instantiate(Resources.Load<GameObject>("Prefabs/CandidateBox"), CandidatesPanel.transform);
                nameBox.GetComponent<CandidateBox>().InitBox(candidate.Key.Data, candidate.Key.Points);
            }            
        }

        EditButton.interactable = false;
        UndoButton.interactable = false;
        CompareButton.interactable = false;

        EditButton.onClick.RemoveAllListeners();
        UndoButton.onClick.RemoveAllListeners();
        CompareButton.onClick.RemoveAllListeners();

        EditButton.onClick.AddListener(OnEditClick);
        UndoButton.onClick.AddListener(OnUndoClick);
        CompareButton.onClick.AddListener(OnCompareClick);
    }

    public void RemoveBoxes()
    {
        for (int i = 0; i < CandidatesPanel.transform.childCount; i++)
        {
            Destroy(CandidatesPanel.transform.GetChild(i).gameObject);
        }
    }

    public void SelectCandidateBox(CandidateBox box)
    {
        if (PreviousBox == null)
            PreviousBox = box;
        else
            PreviousBox = CurrentBox;

        CurrentBox = box;
        boxSelected = true;

        if (PreviousBox != null)
            PreviousBox.gameObject.GetComponent<Outline>().effectColor = Color.black;

        CurrentBox.gameObject.GetComponent<Outline>().effectColor = Color.green;

        EditButton.interactable = boxSelected;
        UndoButton.interactable = boxSelected;
        CompareButton.interactable = boxSelected;
    }

    private void OnEditClick()
    {
        UIHandler.Instance.DispatchPopUp("Edit candidate", "Edit the candiate points you initially delegated to this IGN's entry.", true, true, OptionType.CandEdit, "Enter new points...");
    }

    private void OnUndoClick()
    {
        foreach(var candidate in DataManager.Instance.Candidates)
        {
            if (CurrentBox.FSData.IGN == candidate.Key.IGN)
            {
                Candidate removedCand = candidate.Key;
                DataManager.Instance.Candidates.Remove(removedCand);
                Destroy(CurrentBox.gameObject);
                CurrentBox = null;
                DataManager.Instance.SaveCurrentData();
                return;
            }
        }
    }

    private void OnCompareClick()
    {
        // TODO: Implement something here
    }

    public void UpdateCurrentCandidate(int points)
    {
        CurrentBox.UpdatePoints(points);
        foreach(var candidate in DataManager.Instance.Candidates)
        {
            if (candidate.Key.IGN == CurrentBox.FSData.IGN)
            {
                candidate.Key.Points = points;
            }
        }
        DataManager.Instance.SaveCurrentData();
    }
}