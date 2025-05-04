using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class FlagControl : MonoBehaviour
{
	private const float PointArea = 1;

	[SerializeField] private Flag _flagPrefab;
	[SerializeField] private Transform _flagProjectionPrefab;
	[SerializeField] private TowerFactory _towerFactory;
	[Space]
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private RaycastDetector _raycast;

	private Flag _flag;
	private Transform _flagProjection;
	private Vector3 _hidePoint = new Vector3(9999, 9999, 9999);

	private bool _isCreated = false;
	private bool _isActive = false;

	public Flag Flag => _flag;
	public TowerFactory TowerFactory => _towerFactory;
	public bool IsActive => _isActive;

	public event System.Action Created;

	private void Start()
	{
		_flag = Instantiate(_flagPrefab);
		_flag.Initializate(_towerFactory);
		_flag.gameObject.SetActive(false);

		_flagProjection = Instantiate(_flagProjectionPrefab);
		_flagProjection.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		_inputReader.InputActions.UI.Click.performed += Instance;
	}

	private void OnDisable()
	{
		if (_flag != null)
			_flag.Activeted -= Return;

		_inputReader.InputActions.UI.Click.performed -= Instance;
	}

	private void Update()
	{
		if (_flagProjection.gameObject.activeSelf == false)
			return;

		if (TryGetNavMeshPointUnderMouse(out Vector3 point))
		{
			_flagProjection.transform.position = point;
		}
	}

	public void Initializate(TowerFactory towerFactory, InputReader inputReader, RaycastDetector raycast)
	{
		_towerFactory = towerFactory;
		_inputReader = inputReader;
		_raycast = raycast;
	}

	public void Activate()
	{
		if (_isActive || _isCreated)
			return;

		_flagProjection.gameObject.SetActive(true);
		_flagProjection.transform.position = _hidePoint;

		StartCoroutine(DelayActive());
	}

	public void Instance(InputAction.CallbackContext context)
	{
		if (_isActive == false || _isCreated)
			return;

		_isActive = false;
		_flagProjection.gameObject.SetActive(false);

		if (TryGetNavMeshPointUnderMouse(out Vector3 point))
		{
			_flag.gameObject.SetActive(true);
			_isCreated = true;

			_flag.transform.position = point;
			Created?.Invoke();

			_flag.Activeted += Return;
		}
	}

	private IEnumerator DelayActive() // чтобы не срабатывало разрешение на клик слишком рано
	{
		yield return new WaitForFixedUpdate();

		_isActive = true;
	}

	private void Return()
	{
		_flag.Activeted -= Return;
		_flag.gameObject.SetActive(false);

		_isActive = false;
		_isCreated = false;
	}

	private bool TryGetNavMeshPointUnderMouse(out Vector3 result)
	{
		result = Vector3.zero;

		if (_raycast.TryHitClick(out RaycastHit hit))
		{
			if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, PointArea, NavMesh.AllAreas))
			{
				result = navHit.position;

				return true;
			}
		}

		return false;
	}
}
