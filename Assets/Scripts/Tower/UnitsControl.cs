using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsControl : MonoBehaviour
{
	[SerializeField] private UnitFinder _unitPrefab;
	[SerializeField] private Transform[] _spawnPoints;
	[SerializeField] private Transform _homeTransform;

	private List<UnitFinder> _units = new List<UnitFinder>();

	private int _currentSpawnPoint;

	public void Create(int unitsCount)
	{
		for (int i = 0; i < unitsCount; i++)
		{
			UnitFinder unit = Instantiate(_unitPrefab, NextSpawnPoint(), Quaternion.identity);
			unit.Initializate(_homeTransform);

			_units.Add(unit);
		}
	}

	public void SetUnit(UnitFinder unit)
	{
		_units.Add(unit);
		unit.GetBuildUnit(_homeTransform);
	}

	public void SetTarget(Vector3 targetPosition)
	{
		foreach (var unit in _units)
		{
			if (unit.CurrentTarget == TargetAgent.Stay)
			{
				unit.SetTarget(targetPosition);

				return;
			}
		}
	}

	public bool HaveCollect(Transform collect)
	{
		foreach (var unit in _units)
		{
			if (unit.HoldCollect == collect)
				return true;
		}

		return false;
	}

	public void TaskCreateBuild(Vector3 point)
	{
		StartCoroutine(TargetFreeUnit(point));
	}

	private IEnumerator TargetFreeUnit(Vector3 point)
	{
		UnitFinder unit = _units[Random.Range(0, _units.Count)];

		yield return new WaitUntil(() => unit.CurrentTarget == TargetAgent.Stay);

		CreateBuild(unit, point);
	}

	private void CreateBuild(UnitFinder unit, Vector3 point)
	{
		unit.SetTarget(point);
		unit.SetTaskBuild();

		_units.Remove(unit);
	}

	private Vector3 NextSpawnPoint()
	{
		_currentSpawnPoint++;

		if (_currentSpawnPoint >= _spawnPoints.Length)
		{
			_currentSpawnPoint = 0;
		}

		return _spawnPoints[_currentSpawnPoint].position;
	}
}
