using System;
using System.Collections;
using UnityEngine;

public class ActivatorSkill : MonoBehaviour
{
    [SerializeField] private Skill _skill;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Character _character;


    private Timer _timerDuration;
    private Timer _timerCooldown;
    private Coroutine _jobTimerDuration;
    private Coroutine _jobTimerCooldown;

    public event Action<ActivatorSkill> Activated;
    public event Action<ActivatorSkill> Deactivated;

    public IReadOnlyTimer TimerDuration => _timerDuration;
    public IReadOnlyTimer TimerCooldown => _timerCooldown;
    public Skill Skill => _skill;

    private void OnValidate()
    {
        if (_skill == null)
            return;

        if (_character == null)
            return;

        if (_skill.TryValidate(_character, out string warning))
            return;

        Debug.LogWarning(warning);
        _skill = null;
    }

    private void Awake()
    {
        _timerDuration = new Timer(_skill.Duration);
        _timerCooldown = new Timer(_skill.Cooldown);
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
        if (_skill.TryActivate(_character) == false)
        {
            throw new InvalidOperationException($"Ошибка активации {nameof(_skill)} у {nameof(_character)}");
        }

        Activated?.Invoke(this);
    }

    private void Deactivate()
    {
        if (_skill.TryDeactivate(_character) == false)
        {
            throw new InvalidOperationException($"Ошибка деактивации {nameof(_skill)} у {nameof(_character)}");
        }

        Deactivated?.Invoke(this);
    }
}