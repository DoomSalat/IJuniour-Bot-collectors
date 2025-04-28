using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class FlagControl : MonoBehaviour
{
	private const float PointArea = 1;

	[SerializeField] private Flag _flagPrefab;
	[SerializeField] private Transform _flagProjectionPrefab;
	[Space]
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private float _maxDistance = 100f;

	private MainInputSystem _inputSystem;
	private Camera _camera;
	public Flag _flag;
	public Transform _flagProjection;
	private Vector3 _farPoint = new Vector3(9999, 9999, 9999);

	private bool _isCreated = false;
	private bool _isActive = false;

	public Flag Flag => _flag;
	public bool IsCreated => _isCreated;
	public bool IsActive => _isActive;

	public event System.Action Created;

	private void Awake()
	{
		_inputSystem = new MainInputSystem();

		_camera = Camera.main;
	}

	private void Start()
	{
		_flag = Instantiate(_flagPrefab);
		_flag.gameObject.SetActive(false);

		_flagProjection = Instantiate(_flagProjectionPrefab);
		_flagProjection.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		_inputSystem.UI.Click.performed += Instance;

		_inputSystem.Enable();
	}

	private void OnDisable()
	{
		if (_flag != null)
			_flag.Builded -= Return;

		_inputSystem.UI.Click.performed -= Instance;

		_inputSystem.Disable();
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

	public void Activate()
	{
		if (_isActive == true || _isCreated)
			return;

		_flagProjection.gameObject.SetActive(true);
		_flagProjection.transform.position = _farPoint;

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

			_flag.Builded += Return;
		}
	}

	private IEnumerator DelayActive() // чтобы не срабатывало разрешение на клик слишком рано
	{
		yield return new WaitForFixedUpdate();

		_isActive = true;
	}

	private void Return()
	{
		_flag.Builded -= Return;
		_flag.gameObject.SetActive(false);

		_isActive = false;
		_isCreated = false;
	}

	private bool TryGetNavMeshPointUnderMouse(out Vector3 result)
	{
		result = Vector3.zero;

		Vector2 mousePosition = Mouse.current.position.ReadValue();
		Ray ray = _camera.ScreenPointToRay(mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _groundLayer))
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
