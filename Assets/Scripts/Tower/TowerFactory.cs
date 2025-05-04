using UnityEngine;

public class TowerFactory : MonoBehaviour
{
	[SerializeField] private Tower _towerPrefab;

	public void CreateBuild(UnitFinder unitBuild, Vector3 createPoint)
	{
		Tower tower = Instantiate(_towerPrefab, createPoint, Quaternion.identity);
		tower.TakeUnit(unitBuild);
	}
}
