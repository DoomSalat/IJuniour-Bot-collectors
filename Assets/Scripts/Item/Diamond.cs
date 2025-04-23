using UnityEngine;

public class Diamond : PooledObject, ICollectible
{
	public void Collect() { }

	public void TakeOut()
	{
		Return();
	}
}
