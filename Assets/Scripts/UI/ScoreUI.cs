using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private CoinCounter _coinCounter;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _coinCounter.UpdatedCoins += OnUpdatedCoins;
    }

    private void OnDisable()
    {
        _coinCounter.UpdatedCoins -= OnUpdatedCoins;
    }

    private void OnUpdatedCoins(int value)
    {
        _text.text = value.ToString();
    }
}
