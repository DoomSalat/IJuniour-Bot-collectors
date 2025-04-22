using UnityEngine;

public class LookOnCam : MonoBehaviour
{
	private Camera _mainCamera;

	private void Awake()
	{
		_mainCamera = Camera.main;
	}

	private void Update()
	{
		transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
						 _mainCamera.transform.rotation * Vector3.up);
	}
}
