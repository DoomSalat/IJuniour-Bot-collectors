using UnityEngine;

public class Flag : MonoBehaviour
{
	private TowerFactory _towerFactory;

	public event System.Action Activeted;

	public void Initializate()
	{
		_towerFactory = FindFirstObjectByType<TowerFactory>();
	}

	public void Activate(UnitFinder unitFinder)
	{
		_towerFactory.CreateBuild(unitFinder, transform.position);

		Activeted?.Invoke();
	}
}
