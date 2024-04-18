using System;
using UnityEngine;

public interface IDamageable
{
    Vector3 Position { get; }

    event Action<IDamageDealer> TookDamage;

    void TakeDamage(IDamageDealer damageDealer);
}