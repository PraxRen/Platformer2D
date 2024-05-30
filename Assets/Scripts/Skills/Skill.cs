using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public abstract class Skill : ScriptableObject
{
    [SerializeField] private float _duration = 6f;
    [SerializeField] private Sprite _icon; 

    public float Duration => _duration;
    public Sprite Icon => _icon;

    public abstract bool CanActivate(Player player);

    public abstract void Activate(Player player);

    public abstract void Deactivate(Player player);
}