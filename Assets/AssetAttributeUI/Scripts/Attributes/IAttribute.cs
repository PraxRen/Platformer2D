using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttribute
{
    public event Action OnValueChanged;

    public float MaxValue { get; }
    public float Value { get; }
}
