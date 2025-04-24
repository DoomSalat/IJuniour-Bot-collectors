using System.Collections.Generic;
using UnityEngine;

public class SearchCollects : MonoBehaviour
{
	[SerializeField] private float _detectionRadius = 100f;
	private Collider[] _colliderBuffer = new Collider[32];
	private List<ICollectible> _collectiblesCache = new List<ICollectible>();

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, _detectionRadius);
	}

	public ICollectible[] GetCollectiblesInRange()
	{
		_collectiblesCache.Clear();

		int count = Physics.OverlapSphereNonAlloc(
			transform.position,
			_detectionRadius,
			_colliderBuffer
		);

		if (count >= _colliderBuffer.Length)
		{
			ResizeBuffer(count);
			count = Physics.OverlapSphereNonAlloc(
				transform.position,
				_detectionRadius,
				_colliderBuffer
			);
		}

		for (int i = 0; i < count; i++)
		{
			if (_colliderBuffer[i].TryGetComponent<ICollectible>(out var collectible))
			{
				_collectiblesCache.Add(collectible);
			}
		}

		return _collectiblesCache.ToArray();
	}

	private void ResizeBuffer(int requiredSize)
	{
		int newSize = Mathf.NextPowerOfTwo(requiredSize);
		_colliderBuffer = new Collider[newSize];
	}
}