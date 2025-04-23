using UnityEngine;

public class PooledObject : MonoBehaviour
{
	public event System.Action Returned;

	public void Return()
	{
		Returned?.Invoke();
	}
}
