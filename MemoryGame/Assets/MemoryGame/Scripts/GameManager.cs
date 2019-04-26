using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	private GameRunnerLogics gameRunnerLogics;
	[SerializeField] private GameRunnerGraphics gameRunnerGraphics;
	[SerializeField] private GameConfig gameConfig;
	[SerializeField] private Text timerText;
	[SerializeField] private float targetTime = 60.0f;
	
	private void Awake()
	{
		gameRunnerLogics = new GameRunnerLogics(gameConfig);
		//todo: create game data savior 
		gameRunnerGraphics.InitializedEnvironment(gameRunnerLogics.GetBoard());
		StartGame();
	}

	private void StartGame()
	{
		//1. start a clock
		//2. create main cycle (player choose a card, then choose another
		//3. end game - 
		StartClock();
	}

	private void StartClock()
	{
		timerText.text = FloatTimeToString(targetTime);
	}

	private string FloatTimeToString(float time)
	{
		string minutes = ((int) time / 60).ToString();
		string seconds = (time % 60).ToString("f2");

		return minutes + ":" + seconds;
	}

	private void Update()
	{
		targetTime -= Time.deltaTime;
		timerText.text = FloatTimeToString(targetTime);
		if (targetTime <= 0.0f)
		{
			EndGame();
		}
	}

	private void EndGame()
	{
		Debug.LogError("Game Ended!");
	}
	
}
