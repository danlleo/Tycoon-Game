using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WorkingProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject _workingProgressBarUI;
    [SerializeField] private Unit _unit;
    [SerializeField] private UnitEconomy _unitEconomy;
    [SerializeField] private UnitLevel _unitLevel;
    [SerializeField] private Image _progressBarForeground;

    private float _chanceToFinishWorkWithTroubles = .15f;
    private float _maxTimeInSeconds = 5f;
    private float _normalizedTime = 0f;

    private Coroutine _countDownCoroutine;

    private void Awake()
    {
        HideUI();
    }

    private void Start()
    {
        SetMaxWorkingTime();

        _unit.OnUnitPerformedWorkPiece += Unit_OnUnitPerformedWorkPiece;
        _unitEconomy.OnUnitReceivedMoney += UnitEconomy_OnUnitRecievedMoney;
        DayManager.Instance.OnDayEnded += DayManager_OnDayEnded;
        ResolveWorkIssueUI.OnResolvingFailedWorkIssue += ResolveWorkIssueUI_OnResolvingFailedWorkIssue;
    }

    private void OnDestroy()
    {
        _unit.OnUnitPerformedWorkPiece -= Unit_OnUnitPerformedWorkPiece;
        _unitEconomy.OnUnitReceivedMoney -= UnitEconomy_OnUnitRecievedMoney;
        DayManager.Instance.OnDayEnded -= DayManager_OnDayEnded;
        ResolveWorkIssueUI.OnResolvingFailedWorkIssue -= ResolveWorkIssueUI_OnResolvingFailedWorkIssue;
    }

    private void SetMaxWorkingTime() => _maxTimeInSeconds -= (_unitLevel.GetDependingLevelWorkingSpeedBoost() / 100) * _maxTimeInSeconds;

    private void DayManager_OnDayEnded(object sender, EventArgs e)
    {
        if (_countDownCoroutine != null)
        {
            StopCoroutine(_countDownCoroutine);
            _countDownCoroutine = null;
        }

        HideUI();
    }

    private void ResolveWorkIssueUI_OnResolvingFailedWorkIssue(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;

        if (ReferenceEquals(unit, _unit))
        {
            ShowUI();
            InvokeTimer();
        }
    }

    private void UnitEconomy_OnUnitRecievedMoney(object sender, EventArgs e)
    {
        UnitEconomy unitEconomy = (UnitEconomy)sender;

        if (ReferenceEquals(unitEconomy, _unitEconomy))
        {
            ShowUI();
            InvokeTimer();
        }
    }

    private void Unit_OnUnitPerformedWorkPiece(object sender, EventArgs e)
    {
        Unit unit = (Unit)sender;

        if (ReferenceEquals(unit, _unit))
        {
            ShowUI();
            InvokeTimer();
        }
    }

    public void InvokeTimer() => _countDownCoroutine = StartCoroutine(TimerCountDownInSecondsRoutine());

    private IEnumerator TimerCountDownInSecondsRoutine()
    {
        float timer = 0f;

        while (timer <= _maxTimeInSeconds)
        {
            timer += Time.deltaTime;
            _normalizedTime = timer / _maxTimeInSeconds;
            _progressBarForeground.fillAmount = _normalizedTime;
            yield return null;
        }

        bool hasUnitSuccessfullyFinishedWork = UnityEngine.Random.value > _chanceToFinishWorkWithTroubles;

        if (hasUnitSuccessfullyFinishedWork)
        {
            SoundManager.Instance.PlayUnitSuccess();
        }
        else
        {
            SoundManager.Instance.PlayUnitFail();
        }

        _unitEconomy.InvokeOnUnitReadyToReceiveMoney(hasUnitSuccessfullyFinishedWork);
        HideUI();
    }

    private void ShowUI() => _workingProgressBarUI.SetActive(true);

    private void HideUI() => _workingProgressBarUI.SetActive(false);
}
