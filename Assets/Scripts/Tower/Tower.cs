using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour, IClickable
{
	[SerializeField] private TowerAnimator _animator;
	[SerializeField] private Score _score;

	[Header("Pararmeters")]
	[SerializeField][Min(0)] private float _timeSearch = 1.5f;

	private Coroutine _searchRoutine;
	private WaitForSeconds _delaySearch;

	private void Awake()
	{
		_delaySearch = new WaitForSeconds(_timeSearch);
	}

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
			_searchRoutine = StartCoroutine(DelaySearch());
		}
	}

	private IEnumerator DelaySearch()
	{
		Search();

		yield return _delaySearch;

		_searchRoutine = null;
	}
}
