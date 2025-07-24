using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private float targetTimeScale = 1f;

    private float timeScaleLerpSpeed = 5f;

    private Coroutine timeEffectCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void Update()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * timeScaleLerpSpeed);

        if (Mathf.Abs(Time.timeScale - targetTimeScale) < 0.01f)
        {
            Time.timeScale = targetTimeScale;
        }

        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }


    public void SetTargetTimeScale(float targetTimeScale, float lerpSpeed=5f)
    {
        this.targetTimeScale = targetTimeScale;
        this.timeScaleLerpSpeed = lerpSpeed;
    }

    public void DoSlowMotion(float slowScale, float duration)
    {
        if (timeEffectCoroutine != null)
        {
            StopCoroutine(timeEffectCoroutine);
        }

        timeEffectCoroutine = StartCoroutine(SlowMotionEffect(slowScale, duration));
    }

    private IEnumerator SlowMotionEffect(float slowScale, float duration, float inLerpSpeed = 2f, float outLerpSpeed = 5f)
    {
        SetTargetTimeScale(slowScale, inLerpSpeed);
        yield return new WaitForSecondsRealtime(duration);
        SetTargetTimeScale(1f, outLerpSpeed);
    }
}
