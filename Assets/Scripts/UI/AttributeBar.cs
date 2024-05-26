using UnityEngine;
using UnityEngine.UI;

public class AttributeBar : AttributeUI
{
    [SerializeField] protected Slider Slider;

    protected override void ValueChanged()
    {
        Slider.value = Attribute.Value / Attribute.MaxValue;
    }
}