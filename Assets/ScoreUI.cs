using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private ScoreManager _scoreManager;

    private void OnEnable()
    {
        if (_scoreManager != null)
        {
            _scoreManager.UpdatedCoins += OnUpdatedCoins;
        }        
    }

    private void OnDisable()
    {
        _scoreManager.UpdatedCoins -= OnUpdatedCoins;
    }

    private void Start()
    {
        _scoreManager = ScoreManager.Instance;
        _scoreManager.UpdatedCoins += OnUpdatedCoins;
    }

    private void OnUpdatedCoins(int value)
    {
        _text.text = value.ToString();
    }
}
