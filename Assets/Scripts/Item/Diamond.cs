using UnityEngine;

public class Diamond : PooledObject, ICollectible
{
	public Transform ObjectTransform => transform;

	public void Collect() { }

	public void TakeOut()
	{
		Return();
	}
}
