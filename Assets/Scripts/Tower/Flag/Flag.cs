using UnityEngine;

public class Flag : MonoBehaviour
{
	[SerializeField] private Tower _towerPrefab;

	public event System.Action Builded;

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
