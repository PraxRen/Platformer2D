using System;

public interface IReadOnlyTimer
{
    public event Action Started;
    public event Action Expired;
    public event Action<float> Changed;

    public float StartTime { get; }
    public float Time { get; }
}