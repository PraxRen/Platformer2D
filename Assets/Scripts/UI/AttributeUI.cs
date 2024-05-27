using UnityEngine;

public abstract class AttributeUI : MonoBehaviour
{
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
        Attribute.ValueChanged += ValueChanged;
    }

    private void OnDisable()
    {
        Attribute.ValueChanged -= ValueChanged;
    }

    protected abstract void ValueChanged();
}