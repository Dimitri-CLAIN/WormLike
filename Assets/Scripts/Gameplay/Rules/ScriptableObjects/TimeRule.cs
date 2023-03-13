using System.Collections;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// This class is used for Rules that have a time(seconds) argument.
/// ! RaiseEvent need to be call as param of StartCoroutine !
/// Example: An match length rule
/// </summary>

[CreateAssetMenu(menuName = "Rules/Time")]
public class TimeRuleSO : DescriptionBaseSO, IBasicRule<float>
{
    public event UnityAction OnEventRaised;

    private float _defaultsValue;
    private float _value;

    float IBasicRule<float>.DefaultsValue => _defaultsValue;
    float IBasicRule<float>.Value => _value;

    public void UpdateValue(float newValue)
    {
        if (newValue != 0)
        {
            _value = newValue;
        }
    }

    public void SetAsDefaultValue()
    {
        _value = _defaultsValue;
    }

    public IEnumerator RaiseEvent()
    {
        if (OnEventRaised == null) { yield return null; }

        yield return new WaitForSeconds(_value);
        OnEventRaised?.Invoke();
    }
}