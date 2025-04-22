using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class TopDownCameraMovement : MonoBehaviour
{
	[SerializeField] private InputHandler _inputHandler;

	[Header("Movement Settings")]
	[SerializeField] private float _moveSpeed = 10f;
	[SerializeField] private float _zoomSpeed = 5f;
	[SerializeField] private float _rotationSpeed = 50f;

	[Header("Zoom Clamp")]
	[SerializeField] private float _minZoom = 5f;
	[SerializeField] private float _maxZoom = 50f;

	private Vector2 _moveInput;
	private float _zoomInput;
	private float _rotateInput;

	private Transform _cameraTransform;

	private void Awake()
	{
		_cameraTransform = transform;
	}

	private void OnEnable()
	{
		_inputHandler.InputSystem.Player.Move.performed += OnMove;
		_inputHandler.InputSystem.Player.Move.canceled += OnMoveStop;

		if (_inputHandler.InputSystem.Player.Rotate != null)
		{
			_inputHandler.InputSystem.Player.Rotate.performed += OnRotate;
			_inputHandler.InputSystem.Player.Rotate.canceled += OnRotateStop;
		}

		if (_inputHandler.InputSystem.Player.Zoom != null)
		{
			_inputHandler.InputSystem.Player.Zoom.performed += OnZoom;
		}

		_inputHandler.InputSystem.Enable();
	}

	private void OnDisable()
	{
		_inputHandler.InputSystem.Player.Move.performed -= OnMove;
		_inputHandler.InputSystem.Player.Move.canceled -= OnMoveStop;

		if (_inputHandler.InputSystem.Player.Rotate != null)
		{
			_inputHandler.InputSystem.Player.Rotate.performed -= OnRotate;
			_inputHandler.InputSystem.Player.Rotate.canceled -= OnRotateStop;
		}

		if (_inputHandler.InputSystem.Player.Zoom != null)
		{
			_inputHandler.InputSystem.Player.Zoom.performed -= OnZoom;
		}

		_inputHandler.InputSystem.Disable();
	}

	private void Update()
	{
		Vector3 movement = new Vector3(_moveInput.x, 0, _moveInput.y) * _moveSpeed * Time.deltaTime;
		_cameraTransform.Translate(movement, Space.World);

		if (_rotateInput != 0)
		{
			float rotationAmount = _rotateInput * _rotationSpeed * Time.deltaTime;
			_cameraTransform.Rotate(Vector3.up, rotationAmount, Space.World);
		}

		if (_zoomInput != 0)
		{
			float newY = Mathf.Clamp(_cameraTransform.position.y - _zoomInput * _zoomSpeed * Time.deltaTime, _minZoom, _maxZoom);
			_cameraTransform.position = new Vector3(_cameraTransform.position.x, newY, _cameraTransform.position.z);
			_zoomInput = 0;
		}
	}

	private void OnMove(InputAction.CallbackContext ctx) => _moveInput = ctx.ReadValue<Vector2>();
	private void OnMoveStop(InputAction.CallbackContext ctx) => _moveInput = Vector2.zero;

	private void OnRotate(InputAction.CallbackContext ctx) => _rotateInput = ctx.ReadValue<float>();
	private void OnRotateStop(InputAction.CallbackContext ctx) => _rotateInput = 0f;

	private void OnZoom(InputAction.CallbackContext ctx) => _zoomInput = ctx.ReadValue<Vector2>().y;
}