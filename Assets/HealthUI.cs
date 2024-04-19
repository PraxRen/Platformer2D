using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _health.ValueChenged += OnValueChenged;
    }

    private void OnDisable()
    {
        _health.ValueChenged -= OnValueChenged;
    }

    private void OnValueChenged(float value)
    {
        _text.text = string.Format("{0:f0}", value);
    }
}
