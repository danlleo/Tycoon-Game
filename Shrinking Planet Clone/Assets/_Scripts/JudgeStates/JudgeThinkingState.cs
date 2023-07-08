using UnityEngine;

public class JudgeThinkingState : JudgeBaseState
{
    private Judge _judge;

    public override void EnterState(JudgeStateManager judgeStateManager)
    {
        Debug.Log("Entered Thinking State");

        if (judgeStateManager.TryGetComponent<Judge>(out Judge judge))
        {
            _judge = judge;
        }

        _judge.InvokeJudgeThinkingEvent();
    }

    public override void UpdateState(JudgeStateManager judgeStateManager)
    {
        if (InputManager.Instance.IsTButtonDownThisFrame())
        {
            JudgeQuestionsManager.Instance.SetRandomQuestion();
            _judge.InvokeJudgeAskingEvent();
        }
    }
}