using System;

public class Timer : IReadOnlyTimer
{
    public event Action Started;
    public event Action Expired;
    public event Action<float> Changed;

    public Timer(float startTime)
    {
        StartTime = startTime;
        Time = startTime;
        IsExpired = true;
    }

    public float StartTime { get; }
    public float Time { get; private set; }
    public bool IsExpired { get; private set; }

    public void Start()
    {
        ResetTime();
        Started?.Invoke();
    }

    public void Tick(float time)
    {
        if (IsExpired)
            return;

        Time -= time;

        if (Time <= 0)
        {
            Time = 0;
            IsExpired = true;
            Expired?.Invoke();
        }

        Changed?.Invoke(Time);
    }

    public void ResetTime()
    {
        Time = StartTime;
        IsExpired = false;
        Changed?.Invoke(Time);
    }
}