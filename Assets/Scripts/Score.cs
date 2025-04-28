using UnityEngine;

public class Score : MonoBehaviour
{
	private int _score = 0;

	public event System.Action<int> Changed;

	private void Start()
	{
		Changed?.Invoke(_score);
	}

	public void SetScore(int value)
	{
		_score = value;

		Changed?.Invoke(_score);
	}
}
