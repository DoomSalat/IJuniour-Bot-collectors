using UnityEngine;
using UnityEngine.InputSystem;

public class RaycastDetector : MonoBehaviour
{
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField][Min(0)] private float _maxDistance = 100f;

	private Camera _mainCamera;

	private void Awake()
	{
		_mainCamera = Camera.main;
	}

	public bool TryHitClick(out RaycastHit hit)
	{
		return Physics.Raycast(
		_mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()),
		out hit,
		_maxDistance,
		_groundLayer
		);
	}
}
