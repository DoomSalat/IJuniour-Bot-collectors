using UnityEngine;

public class GameSpeed : MonoBehaviour
{
	[SerializeField][Min(0)] private float _speed = 1;

	private void Update()
	{
		Time.timeScale = _speed;
	}
}
