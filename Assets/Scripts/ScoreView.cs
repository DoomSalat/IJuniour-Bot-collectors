using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreView : MonoBehaviour
{
	[SerializeField] private Score _score;

	private TextMeshProUGUI _textMesh;

	private void Awake()
	{
		_textMesh = GetComponent<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		_score.Changed += UpdateUI;
	}

	private void OnDisable()
	{
		_score.Changed -= UpdateUI;
	}

	public void UpdateUI(int score)
	{
		_textMesh.text = score.ToString();
	}
}
