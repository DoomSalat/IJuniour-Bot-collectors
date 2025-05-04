using UnityEngine;

public interface ICollectible
{
	public Transform ObjectTransform { get; }

	public void Collect();
	public void TakeOut();
}
