using System.Collections.Generic;
using UnityEngine;

public class SkillsPanelUI : MonoBehaviour
{
    [SerializeField] private SkillUI _prefabSkillUI;
    [SerializeField] private ActivatorSkill[] _activatorsSkill;

    private Dictionary<Skill, SkillUI> _skills = new Dictionary<Skill, SkillUI>();

    private void OnEnable()
    {
        foreach (ActivatorSkill activatorSkill in _activatorsSkill)
        {
            activatorSkill.Activated += AddSkillUI;
            activatorSkill.Deactivated += RemoveSkillUI;
        }
    }

    private void OnDisable()
    {
        foreach (ActivatorSkill activatorSkill in _activatorsSkill)
        {
            activatorSkill.Activated -= AddSkillUI;
            activatorSkill.Deactivated -= RemoveSkillUI;
        }
    }

    private void AddSkillUI(ActivatorSkill activatorSkill)
    {
        if (_skills.ContainsKey(activatorSkill.Skill))
            return;

        SkillUI skillUI = Instantiate(_prefabSkillUI, transform);
        skillUI.Initialize(activatorSkill.Skill, activatorSkill.TimerDuration);
        _skills[activatorSkill.Skill] = skillUI;
    }

    private void RemoveSkillUI(ActivatorSkill activatorSkill)
    {
        if (_skills.ContainsKey(activatorSkill.Skill) == false)
            return;

        Destroy(_skills[activatorSkill.Skill].gameObject);
        _skills.Remove(activatorSkill.Skill);
    }
}