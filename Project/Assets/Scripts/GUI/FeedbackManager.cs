using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    [Header("Feedbacks")]
    public Transform feedbackContainer;
    public GameObject feedbackItem;
    public RectTransform inventoryBackground;


	public void ShowFeedback(string msg)
	{
		if (feedbackContainer.childCount >= 5)
		{
			Destroy(feedbackContainer.GetChild(0).gameObject);
		}

		FeedbackItem feedback = Instantiate(feedbackItem, feedbackContainer).GetComponent<FeedbackItem>();
		feedback.feedbackText.text = msg;
		feedback.transform.SetSiblingIndex(feedbackContainer.childCount);
	}

}
