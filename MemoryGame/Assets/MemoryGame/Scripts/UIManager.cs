using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	private const string END_GAME_WIN_MSG = "YOU WON!";
	private const string END_GAME_LOSE_MSG = "GAME OVER!";
	private const string RESTART = "Restart";
	
	public Action OnStartGame;
	public Action OnSaveAction;
	public Action OnLoadAction;
	
	[SerializeField] private GameObject menu;
	[SerializeField] private Text messageText;
	[SerializeField] private Text buttonText;
	[SerializeField] private GameObject settingsScreen;

	private bool isSettingsShowing = false;

	public void StartNewGame()
	{
		menu.SetActive(false);
		if (OnStartGame != null)
		{
			OnStartGame.Invoke();
		}
	}

	public void OnGameEnd(bool hasWon)
	{

		messageText.text = hasWon ? END_GAME_WIN_MSG : END_GAME_LOSE_MSG;
		buttonText.text = RESTART;
		menu.SetActive(true);
	}

	public void OnSettingsClicked()
	{
		isSettingsShowing = !isSettingsShowing;
		settingsScreen.SetActive(isSettingsShowing);
	}

	public void OnSave()
	{
		Debug.LogError("Save Pressed");
		if (OnSaveAction != null)
		{
			OnSaveAction.Invoke();
		}
	}

	public void OnLoad()
	{
		if (OnLoadAction != null)
		{
			OnLoadAction.Invoke();
		}
	}
}
