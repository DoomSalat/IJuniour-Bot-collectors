using UnityEngine;

public interface ICollectible
{
	public Transform ObjectTransform { get; }

	public bool IsHold { get; }

	public void Collect();
	public void TakeOut();
}
