using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
	private const float OffsetTarget = 0.01f;

	[SerializeField][Min(0)] private float _maxSearchMeshDistance = 5;

	private NavMeshAgent _agent;

	private bool _hasTarget;

	public System.Action TargetReached;

	private void Awake()
	{
		_agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		if (_hasTarget && _agent.pathPending == false && _agent.remainingDistance <= _agent.stoppingDistance &&
			(_agent.hasPath == false || _agent.velocity.sqrMagnitude == 0f))
		{
			_hasTarget = false;
			TargetReached?.Invoke();
		}
	}

	public void SetSmartTarget(Vector3 targetPosition)
	{
		_hasTarget = true;

		NavMeshHit hit;

		if (NavMesh.SamplePosition(targetPosition, out hit, _maxSearchMeshDistance, NavMesh.AllAreas))
		{
			_agent.SetDestination(hit.position);
		}
		else
		{
			Debug.LogWarning("No reachable point near target");
		}
	}

	public void ResetPath()
	{
		_agent.ResetPath();
		_hasTarget = false;
	}
}
