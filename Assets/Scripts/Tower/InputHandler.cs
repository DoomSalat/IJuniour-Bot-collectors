using UnityEngine;

public class InputHandler : MonoBehaviour
{
	private MainInputSystem _inputSystem;

	public MainInputSystem InputSystem => _inputSystem;

	private void Awake()
	{
		_inputSystem = new MainInputSystem();
	}

	private void OnEnable()
	{
		_inputSystem.Enable();
	}

	private void OnDisable()
	{
		_inputSystem.Disable();
	}
}
