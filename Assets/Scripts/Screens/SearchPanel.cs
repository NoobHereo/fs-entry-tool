using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SearchPanel : MonoBehaviour
{
    public TMP_InputField SearchField;
    public GameObject SearchResultPanel;
    public Button CloseButton;

    public void Dispatch(bool visible)
    {
        Debug.Log("Dispatch: " + visible);
        transform.GetChild(0).gameObject.SetActive(visible);

        if (visible)
        {
            CloseButton.onClick.RemoveAllListeners();
            CloseButton.onClick.AddListener(OnCloseClick);
        }
    }

    private void OnCloseClick()
    {
        for (int i = 0; i < SearchResultPanel.transform.childCount; i++)
            Destroy(SearchResultPanel.transform.GetChild(i).gameObject);

        Dispatch(false);
    }

    [SerializeField] private List<string> addedIgns = new List<string>();
    private List<LoadedEntryBox> boxes = new List<LoadedEntryBox>();
    public void OnInputFieldChanged()
    {
        string chars = SearchField.text;
        Debug.Log("input length: " + chars.Length);
        foreach(var fs in DataManager.Instance.AllSeekers)
        {
            if (fs.Key.IGN.Length >= chars.Length && fs.Key.IGN.Substring(0, chars.Length) == chars && !string.IsNullOrEmpty(chars) && !addedIgns.Contains(fs.Key.IGN))
            {
                Debug.Log("yoink: " + fs.Key.IGN);
                GameObject loadedEntryBox = Instantiate(Resources.Load<GameObject>("Prefabs/LoadedEntry"), SearchResultPanel.transform);
                loadedEntryBox.GetComponent<LoadedEntryBox>().InitBox(fs.Key, fs.Value);
                boxes.Add(loadedEntryBox.GetComponent<LoadedEntryBox>());
                addedIgns.Add(fs.Key.IGN);
            }
            else
            {
                // Debug.Log("ehh what");
                for (int i = 0; i < SearchResultPanel.transform.childCount; i++)
                {
                    if (!SearchResultPanel.transform.GetChild(i).GetComponent<LoadedEntryBox>().BoxText.text.Contains(chars))
                    {
                        addedIgns.Remove(fs.Key.IGN);
                        Destroy(SearchResultPanel.transform.GetChild(i).gameObject);
                    }
                }
            }
        }
    }
}