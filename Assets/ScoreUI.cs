using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private —oin—ounter _coin—ounter;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _coin—ounter.UpdatedCoins += OnUpdatedCoins;
    }

    private void OnDisable()
    {
        _coin—ounter.UpdatedCoins -= OnUpdatedCoins;
    }

    private void OnUpdatedCoins(int value)
    {
        _text.text = value.ToString();
    }
}
