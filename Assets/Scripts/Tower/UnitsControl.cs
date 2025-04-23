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

	public void SetTarget(Vector3 targetPosition)
	{
		foreach (var unit in _units)
		{
			if (unit.IsBusy == false)
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

	private Vector3 NextSpawnPoint()
	{
		_currentSpawnPoint++;

		return _spawnPoints[_currentSpawnPoint].position;
	}
}
