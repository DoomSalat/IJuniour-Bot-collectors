using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour, IClickable
{
	private const int Add = 1;

	[SerializeField] private TowerAnimator _animator;
	[SerializeField] private CollectDetector _collectDetector;
	[SerializeField] private SearchCollects _searcher;
	[SerializeField] private UnitsControl _unitsControl;
	[SerializeField] private FlagControl _flagControl;
	[SerializeField] private Score _score;

	[Header("Pararmeters")]
	[SerializeField][Min(0)] private float _timeSearch = 1.5f;
	[SerializeField][Min(0)] private float _timerStartScan = 3f;
	[Space]
	[SerializeField][Min(0)] private int _unitsCreateOnActive = 3;
	[SerializeField][Min(0)] private int _valueNewUnit = 3;
	[SerializeField][Min(0)] private int _valueNewBuild = 5;

	private bool _isBuild;
	private int _storageCount;

	private Coroutine _searchRoutine;
	private WaitForSeconds _delaySearch;

	private int StorageCount
	{
		get { return _storageCount; }
		set
		{
			_storageCount = Mathf.Max(0, value);
		}
	}

	private void Awake()
	{
		_delaySearch = new WaitForSeconds(_timeSearch);
	}

	private void Start()
	{
		_animator.Ready += Activate;
	}

	private void OnEnable()
	{
		_collectDetector.Founded += TakeCollect;
		_flagControl.Created += SetTaskBuild;
	}

	private void OnDisable()
	{
		_collectDetector.Founded -= TakeCollect;
		_flagControl.Created -= SetTaskBuild;

		_animator.Ready -= Activate;
	}

	public void Click()
	{
		if (_flagControl.IsActive == false)
			_flagControl.Activate();
	}

	public void TakeUnit(UnitFinder unit)
	{
		_unitsControl.SetUnit(unit);
	}

	private IEnumerator ScanTask()
	{
		var scanDelay = new WaitForSeconds(_timerStartScan);

		bool isWork = true;

		while (isWork)
		{
			yield return scanDelay;

			StartSearch();
		}
	}

	private void StartSearch()
	{
		if (_searchRoutine == null)
		{
			_searchRoutine = StartCoroutine(DelaySearch());
		}
	}

	private void Activate()
	{
		_animator.Ready -= Activate;

		_unitsControl.Create(_unitsCreateOnActive);

		StartCoroutine(ScanTask());
	}

	private IEnumerator DelaySearch()
	{
		_animator.PlaySearch();

		yield return _delaySearch;

		SetUnitsTask();

		_searchRoutine = null;
	}

	private void SetUnitsTask()
	{
		ICollectible[] collectibles = _searcher.GetCollectiblesInRange();

		for (int i = 0; i < collectibles.Length; i++)
		{
			if (_unitsControl.HaveCollect(collectibles[i].ObjectTransform) == false)
			{
				_unitsControl.SetTarget(collectibles[i].ObjectTransform.position);
			}
		}
	}

	private void SetTaskBuild()
	{
		_isBuild = true;
	}

	private void TakeCollect(ICollectible item)
	{
		StorageCount += Add;

		if (_isBuild)
		{
			if (StorageCount >= _valueNewBuild)
			{
				CreateBuild();
			}
		}
		else
		{
			if (StorageCount >= _valueNewUnit)
			{
				CreateUnit();
			}
		}

		_score.SetScore(StorageCount);
		item.TakeOut();
	}

	private void CreateUnit()
	{
		StorageCount -= _valueNewUnit;
		_unitsControl.Create(Add);
	}

	private void CreateBuild()
	{
		_isBuild = false;

		StorageCount -= _valueNewBuild;
		_unitsControl.TaskCreateBuild(_flagControl.Flag.transform.position);
	}
}
