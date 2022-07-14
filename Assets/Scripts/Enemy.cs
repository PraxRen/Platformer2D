using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Enemy : Personage
{
    public void FoundPlayer()
    {
        if (!TryGetComponent(out WayPointMovement wayPointMovement) || !TryGetComponent(out RayVision rayVision))
            return;

        wayPointMovement.Pursuit(rayVision.Result.transform);
    }

    public void LostPlayer()
    {
        if (TryGetComponent(out WayPointMovement wayPointMovement))
            return;

        wayPointMovement.Patrul();
    }

    protected override void Die()
    {
        base.Die();

        if (TryGetComponent(out WayPointMovement wayPointMovement))
        {
            wayPointMovement.enabled = false;
        }

        if (TryGetComponent(out RayVision rayVision))
        {
            rayVision.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player player) && !IsDead)
        {
            _opponent = player;
            Attack();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Player player))
        {
            _opponent = null;
        }
    }
}
