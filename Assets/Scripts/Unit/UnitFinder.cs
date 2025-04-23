using UnityEngine;

public class UnitFinder : MonoBehaviour
{
	[SerializeField] private Transform _holdPoint;
	[SerializeField] private CollectDetector _collectDetector;
	[SerializeField] private MoveAgent _moveAgent;

	private Transform _homeTarget;
	private Transform _currentCollect;

	private bool _isBusy;
	private TargetAgent _targetAgent = TargetAgent.Stay;

	public bool IsBusy => _isBusy;
	public Transform HoldCollect => _currentCollect;

	private void OnEnable()
	{
		_collectDetector.Founded += TakeCollect;
		_moveAgent.TargetReached += StopBusy;
	}

	private void OnDisable()
	{
		_collectDetector.Founded -= TakeCollect;
		_moveAgent.TargetReached += StopBusy;
	}

	private void Update()
	{
		if (_currentCollect != null)
		{
			if (_currentCollect.parent != _holdPoint)
			{
				ItemLost();
			}
		}
	}

	private void OnDestroy()
	{
		_moveAgent.TargetReached -= ReachCollect;
		_moveAgent.TargetReached -= ReachHome;
	}

	public void Initializate(Transform homeTarget)
	{
		_homeTarget = homeTarget;
	}

	public void SetTarget(Vector3 targetPosition)
	{
		if (_targetAgent == TargetAgent.Stay)
		{
			_targetAgent = TargetAgent.Item;

			_moveAgent.TargetReached += ReachCollect;
		}

		_moveAgent.SetSmartTarget(targetPosition);

		_isBusy = true;
	}

	private void ReachCollect()
	{
		_moveAgent.TargetReached -= ReachCollect;

		if (_currentCollect == null)
		{
			StopBusy();
			SetTargetHome();
		}
	}

	private void ReachHome()
	{
		_moveAgent.TargetReached -= ReachHome;

		_targetAgent = TargetAgent.Stay;
		StopBusy();
	}

	private void TakeCollect(ICollectible item)
	{
		if (_currentCollect != null || item.IsHold == true)
			return;

		_collectDetector.enabled = false;

		Transform takedItem = item.ObjectTransform;

		item.Collect();
		takedItem.SetParent(_holdPoint);
		takedItem.localPosition = Vector3.zero;

		_currentCollect = takedItem;

		SetTargetHome();
	}

	private void SetTargetHome()
	{
		_targetAgent = TargetAgent.Home;

		_moveAgent.TargetReached += ReachHome;
		SetTarget(_homeTarget.position);
	}

	private void ItemLost()
	{
		StopBusy();

		_currentCollect = null;
		_collectDetector.enabled = true;

		SetTargetHome();
	}

	private void StopBusy()
	{
		_isBusy = false;
		_moveAgent.ResetPath();
	}
}
