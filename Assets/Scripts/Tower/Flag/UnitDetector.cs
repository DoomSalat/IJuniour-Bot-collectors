using UnityEngine;

[RequireComponent(typeof(Collider))]
public class UnitDetector : MonoBehaviour
{
	public event System.Action<UnitFinder> Founded;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<UnitFinder>(out var unit))
		{
			Founded?.Invoke(unit);
		}
	}
}
