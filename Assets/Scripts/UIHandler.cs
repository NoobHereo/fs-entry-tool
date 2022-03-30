using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler Instance { get; private set; }
    public Button LoadButton, CandidatesButton, OptionsButton, SummaryButton, ExitButton;
    public Button MenuToggleButton;

    public Menu Menu;
    public Load Load;
    public Candidates Candidates;
    public Options Options;
    public Summary Summary;

    public GameObject MenuBar;
    private bool menuCollapsed = true;
    public Screen CurrentScreen;
    public PopUpPanel PopUpPanel;

    private void Start()
    {
        Instance = this;
        LoadButton.onClick.AddListener(OnLoadClick);
        CandidatesButton.onClick.AddListener(OnCandidatesClick);
        OptionsButton.onClick.AddListener(OnOptionsClick);
        SummaryButton.onClick.AddListener(OnSummaryClick);
        ExitButton.onClick.AddListener(OnExitClick);

        MenuToggleButton.onClick.AddListener(ToggleMenuBar);
        CurrentScreen = Menu;
        ToggleMenuBar();
    }

    /// <summary>
    /// OnClick event for the load button
    /// </summary>
    private void OnLoadClick()
    {
        AutoCollapse();
        ShowLoad();
    }

    /// <summary>
    /// OnClick event for the candidates button
    /// </summary>
    private void OnCandidatesClick()
    {
        AutoCollapse();
        ShowCandidates();
    }

    /// <summary>
    /// OnClick event for the options button
    /// </summary>
    private void OnOptionsClick()
    {
        AutoCollapse();
        ShowOptions();
    }

    /// <summary>
    /// OnClick event for the summary button
    /// </summary>
    private void OnSummaryClick()
    {
        AutoCollapse();
        ShowSummary();
    }

    /// <summary>
    /// OnClick event for the exit button which closes the application
    /// </summary>
    private void OnExitClick()
    {
        AutoCollapse();
        Application.Quit();
    }

    /// <summary>
    /// Dispatches the menu. Usually called OnClick events for back buttons on the other screens
    /// </summary>
    public void ShowMenu()
    {
        CurrentScreen.Dispatch(false);
        CurrentScreen = Menu;
        Menu.Dispatch(true);
    }

    /// <summary>
    /// Dispatches the screen that is instantiated after loading a JSON file
    /// </summary>
    public void ShowLoad()
    {
        if (CurrentScreen == Menu)
            Menu.Dispatch(false);

        // The following should only ever happen if the loaded file is not null
        CurrentScreen.Dispatch(false);
        CurrentScreen = Load;
        Load.Dispatch(true);
        Load.InitData();
    }

    /// <summary>
    /// Dispatches the candidates screen
    /// </summary>
    public void ShowCandidates()
    {
        if (CurrentScreen == Menu)
            Menu.Dispatch(false);

        CurrentScreen.Dispatch(false);
        CurrentScreen = Candidates;
        Candidates.Dispatch(true);
        Candidates.InitData();
    }

    /// <summary>
    /// Dispatches the option screen
    /// </summary>
    public void ShowOptions()
    {
        if (CurrentScreen == Menu)
            Menu.Dispatch(false);

        CurrentScreen.Dispatch(false);
        CurrentScreen = Options;
        Options.Dispatch(true);
        Options.Init();
    }

    /// <summary>
    /// Dispatches the summary screen
    /// </summary>
    public void ShowSummary()
    {
        if (CurrentScreen == Menu)
            Menu.Dispatch(false);

        CurrentScreen.Dispatch(false);
        CurrentScreen = Summary;
        Summary.Dispatch(true);
        Summary.Init();
    }

    private void ToggleMenuBar()
    {
        menuCollapsed = !menuCollapsed;
        if (menuCollapsed)
            ExpandMenu();
        else
            CollapseMenu();
    }

    /// <summary>
    /// Expands the content menu
    /// </summary>
    public void ExpandMenu()
    {
        RectTransform menuRect = MenuBar.GetComponent<RectTransform>();
        menuRect.sizeDelta = new Vector2(530f, 1080f);
        menuRect.anchoredPosition = new Vector3(265f, 0, 0);
        for (int i = 0; i < MenuBar.transform.GetChild(0).childCount; i++)
        {
            RectTransform btnRect = MenuBar.transform.GetChild(0).GetChild(i).GetComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(300f, 90f);
            btnRect.anchoredPosition = new Vector3(215f, btnRect.anchoredPosition.y, 0);
            btnRect.transform.GetChild(0).gameObject.SetActive(true);
            btnRect.gameObject.GetComponent<Outline>().enabled = true;
            Image btnImg = btnRect.gameObject.GetComponent<Image>();
            btnImg.sprite = Resources.Load<Sprite>("UI/Buttons/BlueButton");
            btnImg.color = Color.white;
            Image btnIcon = btnRect.transform.GetChild(1).GetComponent<Image>();
            btnIcon.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Collapses the content menu
    /// </summary>
    public void CollapseMenu()
    {
        RectTransform menuRect = MenuBar.GetComponent<RectTransform>();
        menuRect.sizeDelta = new Vector2(170f, 1080f);
        menuRect.anchoredPosition = new Vector3(85f, 0, 0);
        for (int i = 0; i < MenuBar.transform.GetChild(0).childCount; i++)
        {
            RectTransform btnRect = MenuBar.transform.GetChild(0).GetChild(i).GetComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(170f, 140f);
            btnRect.anchoredPosition = new Vector3(85f, btnRect.anchoredPosition.y, 0);
            btnRect.transform.GetChild(0).gameObject.SetActive(false);
            btnRect.gameObject.GetComponent<Outline>().enabled = false;
            Image btnImg = btnRect.gameObject.GetComponent<Image>();
            btnImg.sprite = Resources.Load<Sprite>("UI/Blank");
            btnImg.color = new Color32(48, 48, 48, 100);
            Image btnIcon = btnRect.transform.GetChild(1).GetComponent<Image>();
            btnIcon.transform.localPosition = Vector3.zero;
            btnIcon.gameObject.SetActive(true);
        }        
    }

    private void AutoCollapse()
    {
        if (menuCollapsed)
        {
            menuCollapsed = false;
            CollapseMenu();
        }
    }

    public void DispatchPopUp(string title, string description, bool inputField, bool dualButtons, OptionType type, string inputFieldText)
    {
        PopUpPanel.Dispatch(true);
        PopUpPanel.InitComponents(title, description, inputField, dualButtons, type, inputFieldText);
    }

    public void UpdateFont(bool customFont)
    {
        Load.SetFont(customFont);
    }
}