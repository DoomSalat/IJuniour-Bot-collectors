using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollectDetector : MonoBehaviour
{
	public event System.Action<ICollectible> Founded;

	private void OnTriggerEnter(Collider other)
	{
		if (other.TryGetComponent<ICollectible>(out var item))
		{
			Founded?.Invoke(item);
		}
	}
}
