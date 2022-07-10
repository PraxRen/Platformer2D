using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Enemy : Personage
{
    public void FoundPlayer()
    {
        if (TryGetComponent(out WayPointMovement wayPointMovement) == false || TryGetComponent(out RayVision playerFind) == false)
            return;

        wayPointMovement.Pursuit(playerFind.Result.transform);
    }

    public void LostPlayer()
    {
        if (TryGetComponent(out WayPointMovement wayPointMovement) == false)
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
        if (collision.collider.TryGetComponent<Player>(out Player player) && IsDead == false)
        {
            player.Hit(base._damage);
            base._animator.SetTrigger("Attack");
        }
    }
}
