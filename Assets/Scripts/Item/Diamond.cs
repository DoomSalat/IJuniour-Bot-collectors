using UnityEngine;

public class Diamond : PooledObject, ICollectible
{
	private bool _isHold;

	public Transform ObjectTransform => transform;

	public bool IsHold => _isHold;

	public void Collect()
	{
		_isHold = true;
	}

	public void TakeOut()
	{
		Return();
		_isHold = false;
	}
}
