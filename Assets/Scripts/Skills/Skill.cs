using UnityEngine;

public abstract class Skill : ScriptableObject
{
    [SerializeField] private float _duration = 6f;
    [SerializeField] private float _cooldown = 10f;
    [SerializeField] private Sprite _icon; 

    public float Duration => _duration;
    public float Cooldown => _cooldown;
    public Sprite Icon => _icon;

    public virtual bool TryValidate(Character character, out string warning)
    {
        warning = "";
        return true;
    }

    public abstract bool TryActivate(Character character);

    public abstract bool TryDeactivate(Character character);
}