using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemDetector : MonoBehaviour
{
	public event System.Action<Item> Founded;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<Item>(out var item))
		{
			Founded?.Invoke(item);
		}
	}
}
