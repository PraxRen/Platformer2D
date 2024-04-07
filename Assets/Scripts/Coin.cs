using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController playerController) == false)
            return;

        ScoreManager.Instance.AddCoin();
        Destroy(gameObject);
    }
}
