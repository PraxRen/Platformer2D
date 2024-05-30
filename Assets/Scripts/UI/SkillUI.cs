using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;
    
    public void Initialize(Skill skill, IReadOnlyTimer timer)
    {
        _image.sprite = skill.Icon;
        timer.Changed += OnTimeChanged;
    }

    private void OnTimeChanged(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        _text.text = timeSpan.ToString("ss':'ff");
    }
}