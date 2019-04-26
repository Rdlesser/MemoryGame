using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameRunnerGraphics: MonoBehaviour
{

	public Action OnTimerEnded;

	[SerializeField] private Vector3 boardInstantiationPoint;
	[SerializeField] private GameObject cardPrefab;
	[SerializeField] private float cardMarginX;
	[SerializeField] private float cardMarginY;
	[SerializeField] private Text timerText;
	private bool finished = false;

	private float cardHeight;
	private float cardWidth;

	public void InitializedEnvironment(eCard[,] board)
	{
		float boardInstantiationPointY = boardInstantiationPoint.y;
		for (int i = 0; i < board.GetLength(0); i++)
		{
			for (int j = 0; j < board.GetLength(1); j++)
			{
				GameObject cardObject = Instantiate(cardPrefab, boardInstantiationPoint, Quaternion.identity, gameObject.transform);
				cardObject.GetComponent<Button>().onClick.AddListener(() =>
				{
					OnCardClick(cardObject);
				});
				cardHeight = cardObject.GetComponent<Image>().sprite.rect.height;
				cardWidth = cardObject.GetComponent<Image>().sprite.rect.width;
				float distanceY = cardHeight + cardMarginY;
				Vector3 movementVector = new Vector3(0, - distanceY, 0);
				boardInstantiationPoint += movementVector;
			}

			float distanceX = cardWidth + cardMarginX;
			boardInstantiationPoint = new Vector3(boardInstantiationPoint.x + distanceX, boardInstantiationPointY, boardInstantiationPoint.z);
		}
	}

	public void OnCardClick(GameObject card)
	{
		// TODO: call card script to be added to the prefab.
		// this will call card.getType and return it to the gamemanager.
		card.gameObject.SetActive((false));
		Debug.LogError("Card Clicked!");
	}

	public void StartClock(float targetTime)
	{
		timerText.text = FloatTimeToString(targetTime);
		StartCoroutine(ClockRoutine(targetTime));
	}

	private string FloatTimeToString(float time)
    {
        string minutes = ((int) time / 60).ToString();
        float secondsLeft = time % 60;
        string seconds = secondsLeft.ToString("N0");

        if (secondsLeft < 10)
        {
	        seconds = "0" + seconds;
        }
        
        return minutes + ":" + seconds;
    }

	private IEnumerator ClockRoutine(float targetTime)
	{
		float timeLeft = targetTime;
		while (timeLeft > 0)
		{
			yield return new WaitForSeconds(1);
			timeLeft--;
			timerText.text = FloatTimeToString(timeLeft);
		}

		if (OnTimerEnded != null)
		{
			OnTimerEnded.Invoke();
		}
		
	}
}
