using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private const float PAUSE_TIME_SCALE = 0f;

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool _isPaused;

    private float _previousTimeScale = 1f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        DayManager.Instance.OnDayEnded += DayManager_OnDayEnded;
        Judge.OnJudgeFinishedJob += Judge_OnJudgeFinishedJob;
    }

    private void OnDestroy()
    {
        DayManager.Instance.OnDayEnded -= DayManager_OnDayEnded;
        Judge.OnJudgeFinishedJob -= Judge_OnJudgeFinishedJob;
    }

    private void Judge_OnJudgeFinishedJob(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    private void DayManager_OnDayEnded(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (!InputManager.Instance.IsPauseButtonDownThisFrame())
            return;

        if (_isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        OnGamePaused?.Invoke(this, EventArgs.Empty);
        _isPaused = true;
        _previousTimeScale = Time.timeScale;
        Time.timeScale = PAUSE_TIME_SCALE;
    }

    public void Resume()
    {
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        _isPaused = false;
        Time.timeScale = _previousTimeScale;
    }
}
