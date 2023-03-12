using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// This class is used for Events that have a string argument.
/// Example: An event to toggle a UI interface
/// </summary>

[CreateAssetMenu(menuName = "Events/String Event Channel")]
public class StringEventChannelSO : DescriptionBaseSO
{
	public event UnityAction<string> OnEventRaised;

	public void RaiseEvent(string value)
	{
		if (OnEventRaised != null)
			OnEventRaised?.Invoke(value);
	}
}
