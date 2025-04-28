using UnityEngine;

public class Flag : MonoBehaviour
{
	[SerializeField] private Tower _towerPrefab;
	[SerializeField] private UnitDetector _unitDetector;

	public event System.Action Builded;

	private void OnEnable()
	{
		_unitDetector.Founded += CreateBuild;
	}

	private void OnDisable()
	{
		_unitDetector.Founded -= CreateBuild;
	}

	public void CreateBuild(UnitFinder unitBuild)
	{
		if (unitBuild.CurrentTarget == TargetAgent.Flag)
		{
			Tower tower = Instantiate(_towerPrefab, transform.position, Quaternion.identity);
			tower.TakeUnit(unitBuild);

			Builded?.Invoke();
		}
	}
}
