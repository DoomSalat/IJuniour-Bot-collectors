using UnityEngine;

public class InputHandler : MonoBehaviour
{
	private MainInputSystem _inputSystem;

	public MainInputSystem InputSystem
	{
		get
		{
			if (_inputSystem == null)
			{
				_inputSystem = new MainInputSystem();
			}

			return _inputSystem;
		}
	}

	private void Awake()
	{
		var system = InputSystem;
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
