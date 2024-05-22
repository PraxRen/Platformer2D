using UnityEngine;
using UnityEngine.UI;

public class AttributeBar : MonoBehaviour
{
    [SerializeField] protected Slider Slider;
    [SerializeField] private MonoBehaviour _attributeMonoBehaviour;

    protected IAttribute Attribute { get; private set; }

    private void OnValidate()
    {
        if (_attributeMonoBehaviour == null || _attributeMonoBehaviour is IAttribute)
            return;

        Debug.LogWarning($"{nameof(_attributeMonoBehaviour)} is not {nameof(IAttribute)}");
        _attributeMonoBehaviour = null;
    }

    private void Awake()
    {
        Attribute = (IAttribute)_attributeMonoBehaviour;
    }

    private void OnEnable()
    {
        Attribute.OnValueChanged += ValueChanged;
    }

    private void OnDisable()
    {
        Attribute.OnValueChanged -= ValueChanged;
    }

    protected virtual void ValueChanged()
    {
        Slider.value = Attribute.Value / Attribute.MaxValue;
    }
}
