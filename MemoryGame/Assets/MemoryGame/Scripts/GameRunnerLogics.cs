using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class GameRunnerLogics
{
	private int width;
	private int height;
	private eCard[,] board;
	private GameConfig gameConfig;
	private bool isFirstChoice = true;
	private eCard firstChoice;
	private eCard secondChoice;
	public Action<bool> OnMatch;

	private int requiredMatchesToWin;

	public void InitializeGameLogic(GameConfig gameConfig)
	{
		width = gameConfig.boardWidth;
		height = gameConfig.boardHeight;
		this.gameConfig = gameConfig;
		
		board = new eCard[width, height];
		FillBoard();
		requiredMatchesToWin = width * height / 2;
	}

	private void FillBoard()
	{
		List<eCard> allCards = new List<eCard>();
		// Populate allCards with the total amount of cards based on the data in cardConfig
		foreach (var cardDef in gameConfig.cardConfig.cardDefinitions)
		{
			for (int i = 0; i < cardDef.appearences; i++)
			{
				allCards.Add(cardDef.cardType);
			}
		}
		// Shuffle the allCards List
		allCards = allCards.OrderBy( x => Random.value ).ToList( );
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				board[i, j] = allCards[0];
				allCards.RemoveAt(0);
			}
		}
	}


	public eCard[,] GetBoard()
	{
		return board;
	}

	public void OnCardClicked(eCard cardType)
	{
		bool successfullMatch = false;
		if (isFirstChoice)
		{
			firstChoice = cardType;
			isFirstChoice = false;
		}
		else
		{
			secondChoice = cardType;
			successfullMatch = firstChoice == secondChoice;
			if (successfullMatch)
			{
				requiredMatchesToWin--;
			}

			if (OnMatch != null)
			{
				OnMatch.Invoke(successfullMatch);
			}

			isFirstChoice = true;

		}
	}

	public bool HasPlayerWon()
	{
		return requiredMatchesToWin == 0;
	}
}
