using UnityEngine;

public class UnitFinder : MonoBehaviour
{
	[SerializeField] private Transform _holdPoint;
	[SerializeField] private CollectDetector _collectDetector;
	[SerializeField] private MoveAgent _moveAgent;

	private Transform _homeTarget;
	private Transform _currentCollect;

	private bool _isBusy;

	public bool IsBusy => _isBusy;
	public Transform HoldCollect => _currentCollect;

	private void OnEnable()
	{
		_collectDetector.Founded += TakeCollect;
	}

	private void OnDisable()
	{
		_collectDetector.Founded -= TakeCollect;
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

	public void Initializate(Transform homeTarget)
	{
		_homeTarget = homeTarget;
	}

	public void SetTarget(Vector3 targetPosition)
	{
		if (_isBusy == false)
		{
			_moveAgent.TargetReached = () =>
			{
				if (_currentCollect == null)
				{
					_isBusy = false;
					SetTargetHome();
				}
			};
		}

		_moveAgent.SetSmartTarget(targetPosition);

		_isBusy = true;
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

	[ContextMenu(nameof(SetTargetHome))]
	private void SetTargetHome()
	{
		SetTarget(_homeTarget.position);
	}

	private void ItemLost()
	{
		_isBusy = false;
		_currentCollect = null;
		_moveAgent.ResetPath();

		_collectDetector.enabled = true;
	}
}
