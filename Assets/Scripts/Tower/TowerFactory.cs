using UnityEngine;

public class TowerFactory : MonoBehaviour
{
	[SerializeField] private Tower _towerPrefab;
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private RaycastDetector _raycast;

	public void CreateBuild(UnitFinder unitBuild, Vector3 createPoint)
	{
		Tower tower = Instantiate(_towerPrefab, null, false);
		tower.Initializate(this, _inputReader, _raycast);
		tower.TakeUnit(unitBuild);

		tower.transform.position = createPoint;
		tower.gameObject.SetActive(true);
	}
}
