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
	
	private void Awake()
	{
		gameRunnerLogics = new GameRunnerLogics(gameConfig);
		//todo: create game data savior 
		StartGame();
	}

	private void StartGame()
	{
		//1. start a clock
		//2. create main cycle (player choose a card, then choose another
		//3. end game - 
		gameRunnerGraphics.InitializedEnvironment(gameRunnerLogics.GetBoard());
		StartClock();
		
	}

	private void StartClock()
	{
		gameRunnerGraphics.OnTimerEnded += OnTimerEnded;
		gameRunnerGraphics.StartClock(gameConfig.targetTime);
	}

	private void OnTimerEnded()
	{
		EndGame();
	}


	private void EndGame()
	{
		Debug.LogError("Game Ended!");
	}
	
}
