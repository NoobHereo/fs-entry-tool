using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CandidateBox : MonoBehaviour
{
    public TextMeshProUGUI Name;
    private Button btn;

    public FutureSeekerData FSData { get; private set; }
    public int Points { get; private set; }

    public void InitBox(FutureSeekerData fs, int points)
    {
        gameObject.name = fs.IGN.ToString();
        FSData = fs;
        Points = points;
        Name.text = FSData.IGN + "(" + points + ")";
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        UIHandler.Instance.Candidates.SelectCandidateBox(this);
    }

    public void UpdatePoints(int points)
    {
        Points = points;        
        Name.text = FSData.IGN + "(" + points + ")";
    }
}