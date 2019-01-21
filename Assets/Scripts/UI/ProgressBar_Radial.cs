using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar_Radial : MonoBehaviour
{
	[SerializeField] private Transform LoadingBar;
	[SerializeField] private Transform TextIndicator;
	[SerializeField] private Transform TextGoal;
	[SerializeField] float currentAmount;
	[SerializeField] float speed;
	private TextMeshProUGUI indicatorText;
	private Image fillImage;
		
	void Awake()
	{
		indicatorText = TextIndicator.GetComponent<TextMeshProUGUI>();
		fillImage = LoadingBar.GetComponent<Image>();
	}
	// Update is called once per frame
	void Update ()
	{
		if(currentAmount < 100)
		{
			TextGoal.gameObject.SetActive(true);
			indicatorText.SetText(((int)currentAmount).ToString()+"%");
		}
		else
		{
			TextGoal.gameObject.SetActive(false);
			indicatorText.SetText("DONE!");
		}
		fillImage.fillAmount = currentAmount / 100;
	}

	public void SetProgress(float progress, Transform whereToBe)
	{
		currentAmount = progress;
	}
}
