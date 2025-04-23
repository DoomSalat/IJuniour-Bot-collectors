using System.Collections.Generic;
using UnityEngine;

public class SearchCollects : MonoBehaviour
{
	public float detectionRadius = 100f;

	public ICollectible[] GetCollectiblesInRange()
	{
		Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

		List<ICollectible> collectibles = new List<ICollectible>();

		foreach (var hit in hits)
		{
			ICollectible c = hit.GetComponent<ICollectible>();
			if (c != null)
			{
				collectibles.Add(c);
			}
		}

		return collectibles.ToArray();
	}
}
