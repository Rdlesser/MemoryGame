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
	[SerializeField] private SoundManager soundManager;

	private const string TIME_REMAINING = "time";
	private const string CARD_COLLECTION = "card_collection";
	private const string REQUIRED_MATCHES = "required_matches";
	
	private GameRunnerLogics gameRunnerLogics;
	private GameDataManager gameDataManager;

	private int timeRemaining;
	
	private void Awake()
	{
		gameRunnerLogics = new GameRunnerLogics();
		gameDataManager = new GameDataManager(gameConfig.shouldSaveDataPersistently);
		
		//Add Listeners
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

	private void LoadGame(int time, string cardCollection, int requiredMatches)
	{
		gameRunnerLogics.InitializeGameLogic(gameConfig, cardCollection, requiredMatches);
		gameRunnerGraphics.InitializeEnvironment(gameRunnerLogics.GetBoard(), cardCollection);
		
		// Stop the clock routine in case there is one
		gameRunnerGraphics.StopClock();
		// Force remove the main menu 
		uIManager.RemoveMenu();
		// Force close the settings menu 
		uIManager.ForceCloseSettings();
		// Force allow user input
		gameRunnerGraphics.AllowUserInput(true);
		// start a new clock routine with the loaded time
		StartClock(time);
	}

	private void StartClock(int time = -1)
	{
		// Add a listener for a "timer ended" event
		gameRunnerGraphics.OnTimerEnded += OnTimerEnded;
		float targetTime;
		
		// If we sent a specific time - let that be the target time (case of load)
		if (time > 0)
		{
			targetTime = time;
		}
		// Otherwise, set the time to the time set in the game configuration
		else
		{
			targetTime = gameConfig.targetTime;
		}
		gameRunnerGraphics.StartClock(targetTime);
	}

	private void OnCardClicked(eCard cardType)
	{
		// Play the card flip sound
		soundManager.OnCardFlipped();
		// Run the card clip logics
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
		// End the game in a loss
		EndGame(false);
	}


	private void EndGame(bool hasWon)
	{
		gameRunnerGraphics.OnGameEnd(hasWon);
		uIManager.OnGameEnd(hasWon);
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
		gameRunnerGraphics.FlipPlayerChoices();
		gameRunnerGraphics.ResetChoices(false);
		gameRunnerLogics.ResetChoices();
		timeRemaining = gameRunnerGraphics.GetRemainingTime();
		gameDataManager.SaveInt(timeRemaining, TIME_REMAINING);
		string cardCollection = gameRunnerGraphics.GetCardCollectionAsString();
		gameDataManager.SaveString(cardCollection, CARD_COLLECTION);
		gameDataManager.SaveInt(gameRunnerLogics.requiredMatchesToWin, REQUIRED_MATCHES);

	}

	public void OnLoad()
	{
		timeRemaining = gameDataManager.LoadInt(TIME_REMAINING);
		string cardCollectionString = gameDataManager.LoadString(CARD_COLLECTION);
		int requiredMatches = gameDataManager.LoadInt(REQUIRED_MATCHES);
		LoadGame(timeRemaining, cardCollectionString, requiredMatches);
		
	}

}
