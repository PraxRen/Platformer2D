using System.Collections;
using UnityEngine;

public class AttributeBarSmooth : AttributeBar
{
    [SerializeField] private float _timeUpdateValueSlider;

    private Coroutine _jobUpdateValueSlider;

    protected override void ValueChanged()
    {
        if (_jobUpdateValueSlider != null)
            StopCoroutine(_jobUpdateValueSlider);

        _jobUpdateValueSlider = StartCoroutine(UpdateValueSlider());
    }

    private IEnumerator UpdateValueSlider()
    {
        float startValue = Slider.value;
        float targetValue = Attribute.Value / Attribute.MaxValue;
        float elapsedTime = 0f;
        float delta = Mathf.Abs(targetValue - startValue);
        float speedUpdate = delta / _timeUpdateValueSlider;

        while (Mathf.Approximately(Slider.value, targetValue) == false)
        {
            Slider.value = Mathf.Lerp(startValue, targetValue, speedUpdate * elapsedTime / delta);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Slider.value = targetValue;
        _jobUpdateValueSlider = null;
    }
}