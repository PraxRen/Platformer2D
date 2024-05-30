using System;
using System.Collections;
using UnityEngine;

public class ActivatorSkill : MonoBehaviour
{
    [SerializeField] private Skill _skill;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Player _player;
    [SerializeField] private float _cooldown = 10f;

    private Timer _timerDuration;
    private Timer _timerCooldown;
    private Coroutine _jobTimerDuration;
    private Coroutine _jobTimerCooldown;

    public event Action<ActivatorSkill> Activated;
    public event Action<ActivatorSkill> Deactivated;

    public IReadOnlyTimer TimerDuration => _timerDuration;
    public IReadOnlyTimer TimerCooldown => _timerCooldown;
    public Skill Skill => _skill;

    private void Awake()
    {
        _timerDuration = new Timer(_skill.Duration);
        _timerCooldown = new Timer(_cooldown);
    }

    private void OnEnable()
    {
        _playerInput.ActivateSkill += Run;
        _timerDuration.Started += StartedTimerDuration;
        _timerDuration.Expired += ExpiredTimerDuration;
    }

    private void OnDisable()
    {
        _playerInput.ActivateSkill -= Run;
        _timerDuration.Started -= StartedTimerDuration;
        _timerDuration.Expired -= ExpiredTimerDuration;
        Deactivate();
        CancelRunTimer(_jobTimerDuration, _timerDuration);
        CancelRunTimer(_jobTimerCooldown, _timerCooldown);
    }

    private void Run()
    {
        if (_timerCooldown.IsExpired == false)
            return;

        if (_timerDuration.IsExpired == false)
            return;

        if (_skill.CanActivate(_player) == false)
            return;

        _jobTimerDuration = StartCoroutine(RunTimer(_timerDuration));
    }

    private IEnumerator RunTimer(Timer timer)
    {
        timer.Start();

        while (timer.IsExpired == false)
        {
            timer.Tick(Time.deltaTime);
            yield return null;
        }
    }

    private void StartedTimerDuration()
    {
        Activate();
    }

    private void ExpiredTimerDuration()
    {
        Deactivate();
        _jobTimerCooldown = StartCoroutine(RunTimer(_timerCooldown));

    }

    private void CancelRunTimer(Coroutine coroutine, Timer timer)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        timer.ResetTime();
    }

    private void Activate()
    {
        _skill.Activate(_player);
        Activated?.Invoke(this);
    }

    private void Deactivate()
    {
        _skill.Deactivate(_player);
        Deactivated?.Invoke(this);
    }
}