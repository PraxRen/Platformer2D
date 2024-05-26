using TMPro;
using UnityEngine;

public class AttributeTextUI : AttributeUI
{
    [SerializeField] private TextMeshProUGUI _text;

    protected override void ValueChanged()
    {
        _text.text = string.Format("{0:f0}/{1:f0}", Attribute.Value, Attribute.MaxValue);
    }
}