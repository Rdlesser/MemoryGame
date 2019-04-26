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
	[SerializeField] private UIManager uIManager;
	
	private void Awake()
	{
		gameRunnerLogics = new GameRunnerLogics();
		uIManager.OnStartGame += StartGame;
		gameRunnerGraphics.cardClickAction += OnCardClicked;
		gameRunnerLogics.OnMatch += OnMatch;
		//todo: create game data savior 
	}

	private void StartGame()
	{
		//1. start a clock
		//2. create main cycle (player choose a card, then choose another
		//3. end game - 
		gameRunnerLogics.InitializeGameLogic(gameConfig);
		gameRunnerGraphics.InitializedEnvironment(gameRunnerLogics.GetBoard());
		StartClock();
	
	}

	private void StartClock()
	{
		gameRunnerGraphics.OnTimerEnded += OnTimerEnded;
		gameRunnerGraphics.StartClock(gameConfig.targetTime);
	}

	private void OnCardClicked(eCard cardType)
	{
		gameRunnerLogics.OnCardClicked(cardType);
	}

	private void OnMatch(bool successfullMatch)
	{
		// Cards don't match
		if (!successfullMatch)
		{
			gameRunnerGraphics.OnMatchFailed();
		}
		// Cards do match
		else
		{
			// Check if game has ended with a win
			if (gameRunnerLogics.HasPlayerWon())
			{
				EndGame(true);
			}
			gameRunnerGraphics.ResetChoices();
		}
	}

	private void OnTimerEnded()
	{
		EndGame(false);
	}


	private void EndGame(bool hasWon)
	{
		gameRunnerGraphics.OnGameEnd(hasWon);
		uIManager.OnGameEnd(hasWon);

		Debug.LogError("Game Ended!");
	}
	
}
