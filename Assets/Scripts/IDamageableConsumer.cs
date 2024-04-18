using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageableConsumer
{
    IDamageable GetDamageable();

    void SetDamageable(IDamageable damageable);
}
