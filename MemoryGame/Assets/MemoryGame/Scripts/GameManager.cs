using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class GameManager : MonoBehaviour
{

	[SerializeField] private GameRunnerGraphics gameRunnerGraphics;
	[SerializeField] private GameConfig gameConfig;
	[SerializeField] private UIManager uIManager;

	private const string TIME_REMAINING = "time";
	private const string CARD_COLLECTION = "card_collection";
	
	private GameRunnerLogics gameRunnerLogics;
	private GameDataManager gameDataManager;

	private int timeRemaining;
	
	private void Awake()
	{
		gameRunnerLogics = new GameRunnerLogics();
		gameDataManager = new GameDataManager(gameConfig.shouldSaveDataPersistently);
		uIManager.OnStartGame += StartGame;
		gameRunnerGraphics.cardClickAction += OnCardClicked;
		gameRunnerLogics.OnMatch += OnMatch;
		uIManager.OnSaveAction += OnSave;
		uIManager.OnLoadAction += OnLoad;
		uIManager.OnSettingsAction += OnSettingsClicked;
	}

	private void StartGame()
	{
		gameRunnerLogics.InitializeGameLogic(gameConfig);
		gameRunnerGraphics.InitializeEnvironment(gameRunnerLogics.GetBoard());
		StartClock();
	
	}

	private void LoadGame(int time, string cardCollection)
	{
		gameRunnerLogics.InitializeGameLogic(gameConfig, cardCollection);
		gameRunnerGraphics.InitializeEnvironment(gameRunnerLogics.GetBoard(), cardCollection);
		gameRunnerGraphics.StopClock();
		StartClock(time);
	}

	private void StartClock(int time = -1)
	{
		gameRunnerGraphics.OnTimerEnded += OnTimerEnded;
		float targetTime;
		if (time > 0)
		{
			targetTime = time;
		}
		else
		{
			targetTime = gameConfig.targetTime;
		}
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

	public void OnSettingsClicked(bool isSettingsShowing)
	{
		if (isSettingsShowing && !uIManager.IsMenuActive())
		{
			timeRemaining = gameRunnerGraphics.StopClock();
			gameRunnerGraphics.AllowUserInput(false);
		}
		else if (!isSettingsShowing && !uIManager.IsMenuActive())
		{
			gameRunnerGraphics.StartClock(timeRemaining);
			gameRunnerGraphics.AllowUserInput(true);
		}
	}

	public void OnSave()
	{
		gameDataManager.SaveInt(timeRemaining, TIME_REMAINING);
		string cardCollection = gameRunnerGraphics.GetCardCollectionAsString();
		gameDataManager.SaveString(cardCollection, CARD_COLLECTION);

	}

	public void OnLoad()
	{
		timeRemaining = gameDataManager.LoadInt(TIME_REMAINING);
		string cardCollectionString = gameDataManager.LoadString(CARD_COLLECTION);
		LoadGame(timeRemaining, cardCollectionString);
		
	}

}
