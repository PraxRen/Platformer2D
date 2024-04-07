using UnityEngine;

public class AidKit : MonoBehaviour
{
    [SerializeField] float _healthPoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController playerController) == false)
            return;

        if (collision.TryGetComponent(out Health health) == false)
            return;

        health.Heal(_healthPoints);
        Destroy(gameObject);
    }
}