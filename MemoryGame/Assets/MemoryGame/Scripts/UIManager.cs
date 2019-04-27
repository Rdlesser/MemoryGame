using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Manager for the UI
 */
public class UIManager : MonoBehaviour
{
	private const string END_GAME_WIN_MSG = "YOU WON!";
	private const string END_GAME_LOSE_MSG = "GAME OVER!";
	private const string RESTART = "Restart";

	private const string SAVE_BUTTON = "SaveButton";
	
	public Action OnStartGame;
	public Action<bool> OnSettingsAction;
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
		if (isSettingsShowing)
		{
			OnSettingsClicked();
		}
		if (OnStartGame != null)
		{
			OnStartGame.Invoke();
		}
	}

	public bool IsMenuActive()
	{
		return menu.activeSelf;
	}
	/*
	 * Force remove the main menu
	 */
	public void RemoveMenu()
	{
		menu.SetActive(false);
	}

	public void OnGameEnd(bool hasWon)
	{

		// Set the text of the menu according to the game result
		messageText.text = hasWon ? END_GAME_WIN_MSG : END_GAME_LOSE_MSG;
		buttonText.text = RESTART;
		menu.SetActive(true);
	}


	public void OnSettingsClicked()
	{
		isSettingsShowing = !isSettingsShowing;
		settingsScreen.SetActive(isSettingsShowing);
		GameObject saveButton = settingsScreen.transform.Find(SAVE_BUTTON).gameObject;
		
		// Case of "settings" clicked from the main menu - don't show 'save game' option
		if (IsMenuActive())
		{
			
			saveButton.SetActive(false);
		}
		// Otherwise - show 'save game' option
		else
		{
			saveButton.SetActive(true);
		}
		if (OnSettingsAction != null)
		{
			OnSettingsAction.Invoke(isSettingsShowing);
		}
	}

	public void ForceCloseSettings()
	{
		isSettingsShowing = false;
		settingsScreen.SetActive(isSettingsShowing);
	}

	public void OnSave()
	{
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
