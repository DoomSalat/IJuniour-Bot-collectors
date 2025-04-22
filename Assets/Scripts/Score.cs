using UnityEngine;

public class Score : MonoBehaviour
{
	private const int Add = 1;

	private int _score = 0;

	public event System.Action<int> Changed;

	private void Start()
	{
		Changed?.Invoke(_score);
	}

	public void AddScore()
	{
		_score += Add;

		Changed?.Invoke(_score);
	}
}
