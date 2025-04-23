using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TowerAnimator : MonoBehaviour
{
	[SerializeField] private ParticleSystem _particleExplose;

	private Animator _animator;

	public event System.Action Ready;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	public void PlaySearch()
	{
		_animator.SetTrigger(TowerAnimatorData.Params.Play);
	}

	public void ParticleExplose()
	{
		_particleExplose.Play();
	}

	public void ReadyState()
	{
		Ready?.Invoke();
	}
}
