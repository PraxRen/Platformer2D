using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _health.Updated += OnUpdated;
    }

    private void OnDisable()
    {
        _health.Updated -= OnUpdated;
    }

    private void OnUpdated(float value)
    {
        _text.text = string.Format("{0:f0}", value);
    }
}
