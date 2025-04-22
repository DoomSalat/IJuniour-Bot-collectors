using UnityEngine;
using UnityEngine.InputSystem;

public class AdvancedClickDetector : MonoBehaviour
{
	[SerializeField] private GameObject _clickableObject;
	[SerializeField] private InputHandler _inputHandler;

	private Camera _mainCamera;
	private Collider _objectCollider;
	private IClickable _clickable;

	private void Awake()
	{
		_mainCamera = Camera.main;
		_objectCollider = GetComponent<Collider>();
		_clickable = _clickableObject.GetComponent<IClickable>();
	}

	private void OnEnable()
	{
		_inputHandler.InputSystem.UI.Click.performed += OnClick;
	}

	private void OnDisable()
	{
		_inputHandler.InputSystem.UI.Click.performed -= OnClick;
	}

	private void OnValidate()
	{
		if (_clickableObject != null && _clickableObject.TryGetComponent<IClickable>(out _) == false)
		{
			_clickableObject = null;
		}
	}

	private void OnClick(InputAction.CallbackContext context)
	{
		Vector2 mousePosition = Mouse.current.position.ReadValue();

		Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			if (hit.collider == _objectCollider)
			{
				_clickable.Click();
			}
		}
	}
}