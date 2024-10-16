using System.Collections;
using UnityEngine;
using Utils;

public class ScreenShake : Singleton<ScreenShake>
{
    private Camera _camera;

    [SerializeField] private float _shakeMagnitude;
    [SerializeField] private float _shakeDuration;
    [SerializeField] private float _shakeReturnToInitialPositionTime;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
    }

    private void Start()
    {
        Judge.Judge.OnJudgeReviewedAnswer += Judge_OnJudgeReviewedAnswer;
    }

    private void OnDestroy()
    {
        Judge.Judge.OnJudgeReviewedAnswer -= Judge_OnJudgeReviewedAnswer;
    }

    private void Judge_OnJudgeReviewedAnswer(object sender, Judge.Judge.ReceivedAnswerArgs e)
    {
        if (e.IsAnswerCorrect)
        {
            InvokeScreenShake();
        }
    }

    public void InvokeScreenShake()
    {
        StartCoroutine(ScreenShakeRoutine());
    }

    private IEnumerator ScreenShakeRoutine()
    {
        float elapsedTime = 0f;

        Vector3 initialPosition = _camera.transform.localPosition;

        while (elapsedTime < _shakeDuration)
        {
            Vector3 randomPoint = Random.insideUnitSphere * _shakeMagnitude;
            _camera.transform.localPosition = new Vector3(initialPosition.x + randomPoint.x,
                initialPosition.y + randomPoint.y, initialPosition.z);
            yield return null;

            elapsedTime += Time.deltaTime;
        }

        elapsedTime = 0f;

        while (elapsedTime <= _shakeReturnToInitialPositionTime)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / _shakeReturnToInitialPositionTime;
            _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, initialPosition,
                InterpolateUtils.EaseInOut(normalizedTime));
        }

        _camera.transform.localPosition = initialPosition;
    }
}
