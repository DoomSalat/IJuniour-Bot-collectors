using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour, IClickable
{
	[SerializeField] private TowerAnimator _animator;
	[SerializeField] private CollectDetector _collectDetector;
	[SerializeField] private SearchCollects _searcher;
	[SerializeField] private UnitsControl _unitsControl;
	[SerializeField][Min(0)] private int _unitsCreateOnActive = 3;
	[SerializeField] private Score _score;

	[Header("Pararmeters")]
	[SerializeField][Min(0)] private float _timeSearch = 1.5f;

	private Coroutine _searchRoutine;
	private WaitForSeconds _delaySearch;

	private void Awake()
	{
		_delaySearch = new WaitForSeconds(_timeSearch);
	}

	private void Start()
	{
		_animator.Ready += Activate;
	}

	private void Activate()
	{
		_animator.Ready -= Activate;

		_unitsControl.Create(_unitsCreateOnActive);
	}

	private void OnEnable()
	{
		_collectDetector.Founded += TakeCollect;
	}

	private void OnDisable()
	{
		_collectDetector.Founded -= TakeCollect;
	}

	public void Search()
	{
		_animator.PlaySearch();
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

		ICollectible[] collectibles = _searcher.GetCollectiblesInRange();

		for (int i = 0; i < collectibles.Length; i++)
		{
			if (_unitsControl.HaveCollect(collectibles[i].ObjectTransform) == false)
			{
				_unitsControl.SetTarget(collectibles[i].ObjectTransform.position);
			}
		}

		_searchRoutine = null;
	}

	private void TakeCollect(ICollectible item)
	{
		_score.AddScore();

		item.TakeOut();
	}
}
