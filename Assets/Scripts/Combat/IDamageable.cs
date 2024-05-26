using System;
using UnityEngine;

public interface IDamageable
{
    Vector3 Position { get; }
    bool IsDied { get; }

    event Action<IDamageDealer> OnTookDamage;

    void TakeDamage(IDamageDealer damageDealer);
}