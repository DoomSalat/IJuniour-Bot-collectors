using UnityEngine;

public class UnitFinder : MonoBehaviour
{
	[SerializeField] private Transform _holdPoint;
	[SerializeField] private CollectDetector _collectDetector;
	[SerializeField] private MoveAgent _moveAgent;
	[Space]
	[SerializeField][Min(0)] private float _activeFlagDistance = 3;

	private Transform _homeTarget;
	private Transform _currentCollect;
	private Flag _flag;

	private TargetAgent _targetAgent = TargetAgent.Stay;

	public TargetAgent CurrentTarget => _targetAgent;

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

		if (_flag != null)
		{
			float distance = (_flag.transform.position - transform.position).magnitude;

			if (distance < _activeFlagDistance)
			{
				_flag.CreateBuild(this);
				_flag = null;

				StopBusy();
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
	}

	public void SetTaskBuild(Flag flag)
	{
		_targetAgent = TargetAgent.Flag;
		_flag = flag;
	}

	public void GetBuildUnit(Transform home)
	{
		_homeTarget = home;
		_targetAgent = TargetAgent.Stay;
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

		_collectDetector.gameObject.SetActive(false);

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
		_collectDetector.gameObject.SetActive(true);

		SetTargetHome();
	}

	private void StopBusy()
	{
		_targetAgent = TargetAgent.Stay;
		_moveAgent.ResetPath();
	}
}
