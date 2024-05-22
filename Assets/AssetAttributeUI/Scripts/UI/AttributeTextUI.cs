using System;
using TMPro;
using UnityEngine;

public class AttributeTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private MonoBehaviour _attributeMonoBehaviour;

    private IAttribute _attribute;

    private void OnValidate()
    {
        if (_attributeMonoBehaviour == null || _attributeMonoBehaviour is IAttribute)
            return;

        Debug.LogWarning($"{nameof(_attributeMonoBehaviour)} is not {nameof(IAttribute)}");
        _attributeMonoBehaviour = null;
    }

    private void Awake()
    {
        _attribute = (IAttribute)_attributeMonoBehaviour;
    }

    private void OnEnable()
    {
        _attribute.OnValueChanged += OnValueChenged;
    }

    private void OnDisable()
    {
        _attribute.OnValueChanged -= OnValueChenged;
    }

    private void OnValueChenged()
    {
        _text.text = string.Format("{0:f0}/{1:f0}", _attribute.Value, _attribute.MaxValue);
    }
}
