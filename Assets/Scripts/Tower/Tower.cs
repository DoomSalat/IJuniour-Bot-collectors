using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour, IClickable
{
	[SerializeField] private TowerAnimator _animator;
	[SerializeField] private Score _score;

	[Header("Pararmeters")]
	[SerializeField][Min(0)] private float _timeSearch = 1.5f;

	private Coroutine _searchRoutine;

	public void Search()
	{
		_animator.PlaySearch();
	}

	public void AddScore()
	{
		_score.AddScore();
	}

	public void Click()
	{
		if (_searchRoutine == null)
		{
			StartCoroutine(DelaySearch());
		}
	}

	private IEnumerator DelaySearch()
	{
		yield return new WaitForSeconds(_timeSearch);

		Search();

		_searchRoutine = null;
	}
}
