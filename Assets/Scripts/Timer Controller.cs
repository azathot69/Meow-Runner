using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerController : MonoBehaviour
{
	private float timeCounter;
	private float countdownTimer = 120f;
	private int minutes;
	private int seconds;
	private bool isCountdown;
	[SerializeField] TMP_Text timerText;

	private void Update () 
	{
		if (isCountdown && countdownTimer > 0)
		{
			countdownTimer -= Time.deltaTime;
			minutes = Mathf.FloorToInt(countdownTimer / 60f);
			seconds = Mathf.FloorToInt(countdownTimer - minutes * 60);
		} 
		else if (!isCountdown)
		{
			timeCounter += Time.deltaTime;
			minutes = Mathf.FloorToInt(timeCounter / 60f);
			seconds = Mathf.FloorToInt(timeCounter - minutes * 60);
		}

		timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
	}
}
