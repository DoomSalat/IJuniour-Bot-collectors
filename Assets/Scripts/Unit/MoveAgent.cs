using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
	[SerializeField][Min(0)] private float _maxSearchMeshDistance = 5;
	[SerializeField][Min(0)] private float _reachDistance = 1.5f;

	private NavMeshAgent _agent;

	private bool _hasTarget;
	private Vector3 _targetPoint;

	public System.Action TargetReached;

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (_hasTarget == false || _agent.pathPending)
			return;

		float distanceToTarget = (_targetPoint - transform.position).sqrMagnitude;

		if (distanceToTarget <= _reachDistance)
		{
			_hasTarget = false;
			TargetReached?.Invoke();
		}
	}

	public void SetSmartTarget(Vector3 targetPosition)
	{
		_targetPoint = targetPosition;
		_hasTarget = true;

		NavMeshHit hit;

		if (NavMesh.SamplePosition(targetPosition, out hit, _maxSearchMeshDistance, NavMesh.AllAreas))
		{
			_agent.SetDestination(hit.position);
		}
		else
		{
			ResetPath();
		}
	}

	public void ResetPath()
	{
		_agent.ResetPath();
		_hasTarget = false;
	}
}
