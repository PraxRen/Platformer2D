using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Player : Personage
{
    private List<Coin> _coins = new List<Coin>();
    
    protected override void Die()
    {
        base.Die();
        
        if(TryGetComponent(out Movement movement))
            movement.enabled = false;
        
        Debug.Log($"{name} погиб!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Enemy>(out Enemy enemy) == true)
        {
            _opponent = enemy;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Enemy>(out Enemy enemy) == true)
        {
            _opponent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin) == true)
        {
            _coins.Add(coin);
            Destroy(coin.gameObject);
        }
    }
}