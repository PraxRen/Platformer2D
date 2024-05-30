using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivatorSkillUI : MonoBehaviour
{
    [SerializeField] private ActivatorSkill _activatorSkill;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color _colorForReady;
    [SerializeField] private Color _colorForNotReady;

    private void Awake()
    {
        _image.color = _colorForReady;
        _text.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _activatorSkill.Activated += OnActivatedSkill;
        _activatorSkill.Deactivated += OnDeactivatedSkill;
    }

    private void OnDisable()
    {
        _activatorSkill.Activated -= OnActivatedSkill;
        _activatorSkill.Deactivated -= OnDeactivatedSkill;
        _activatorSkill.TimerCooldown.Expired -= OnExpiredTimerCooldown;
        _activatorSkill.TimerCooldown.Changed -= OnChangedTimerCooldown;
    }

    private void OnActivatedSkill(ActivatorSkill activatorSkill)
    {
        _image.color = _colorForNotReady;
    }

    private void OnDeactivatedSkill(ActivatorSkill activatorSkill)
    {
        _text.gameObject.SetActive(true);
        _activatorSkill.TimerCooldown.Changed += OnChangedTimerCooldown;
        _activatorSkill.TimerCooldown.Expired += OnExpiredTimerCooldown;
    }

    private void OnExpiredTimerCooldown()
    {
        _activatorSkill.TimerCooldown.Expired -= OnExpiredTimerCooldown;
        _text.gameObject.SetActive(false);
        _image.color = _colorForReady;
    }

    private void OnChangedTimerCooldown(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        _text.text = timeSpan.ToString("ss':'ff");
    }
}