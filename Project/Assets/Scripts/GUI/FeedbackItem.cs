using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeedbackItem : MonoBehaviour
{
	public TextMeshProUGUI feedbackText;
	public float disappearSpeed;

	public void SetFeedbackText(string text)
	{
		feedbackText.text = text;
	}

	private void Update()
	{
		feedbackText.alpha -= Time.deltaTime * (disappearSpeed/10);

		if (feedbackText.alpha <= 0) Destroy(gameObject);
	}

}
