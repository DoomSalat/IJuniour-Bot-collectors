using UnityEngine;
using UnityEngine.InputSystem;

public class AdvancedClickDetector : MonoBehaviour
{
	[SerializeField] private InputReader _inputReader;
	[SerializeField] private RaycastDetector _raycast;

	private void OnEnable()
	{
		_inputReader.InputActions.UI.Click.performed += OnClick;
	}

	private void OnDisable()
	{
		_inputReader.InputActions.UI.Click.performed -= OnClick;
	}

	private void OnClick(InputAction.CallbackContext context)
	{
		if (_raycast.TryHitClick(out RaycastHit hit))
		{
			if (hit.collider.TryGetComponent<IClickable>(out var clickable))
				clickable.Click();
		}
	}
}