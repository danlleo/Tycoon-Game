using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManagingUI : MonoBehaviour
{
    public static event EventHandler OnOpenBuyUI;

    [SerializeField] private GameObject _managingUI;
    [SerializeField] private TextMeshProUGUI _currentCompanyRankText;
    [SerializeField] private TextMeshProUGUI _currentDayText;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _interviewButton;

    private void Awake()
    {
        _startButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.WorkingScene);
        });

        _buyButton.onClick.AddListener(() =>
        {
            Hide();
            OnOpenBuyUI?.Invoke(this, EventArgs.Empty);
        });

        _interviewButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.InterviewScene);
        });
    }

    private void Start()
    {
        _currentCompanyRankText.text = "Company Rank: ";
        _currentDayText.text = "Current Day: " + DayManager.Instance.GetCurrentDay();
        BuyUI.OnOpenBuyUIClose += BuyUI_OnOpenBuyUIClose;

        if (IsInterviewDay())
        {
            ShowInterviewButton();
        }
        else
        {
            ShowStartButton();
        }
    }

    private bool IsInterviewDay() => DayManager.Instance.GetCurrentDay() % 4 == 0;

    private void OnDestroy()
    {
        BuyUI.OnOpenBuyUIClose -= BuyUI_OnOpenBuyUIClose;
    }

    private void BuyUI_OnOpenBuyUIClose(object sender, EventArgs e)
    {
        Show();
    }

    private void Show() => _managingUI.SetActive(true);

    private void Hide() => _managingUI.SetActive(false);

    private void ShowStartButton() => _startButton.gameObject.SetActive(true);

    private void ShowInterviewButton() => _interviewButton.gameObject.SetActive(true);
}
