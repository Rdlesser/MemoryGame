using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Random = UnityEngine.Random;

/*
 * Game logics
 */
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

	public int requiredMatchesToWin { get; set; }

	public void InitializeGameLogic(GameConfig gameConfig, string cardCollectionString = null,
		int requiredMatches = -1)
	{
		width = gameConfig.boardWidth;
		height = gameConfig.boardHeight;
		this.gameConfig = gameConfig;
		
		board = new eCard[width, height];
		List<eCard> cardCollection = ParseCardCollectionFromString(cardCollectionString);
		FillBoard(cardCollection);
		requiredMatchesToWin = requiredMatches > 0? requiredMatches : width * height / 2;
	}

	private List<eCard> ParseCardCollectionFromString(string cardCollectionString)
	{
		List<eCard> cardCollection = null;
		if (cardCollectionString != null)
		{
			cardCollection = new List<eCard>();
			
			for (int i = 0; i < height * width; i++)
			{
				int commaIndex = cardCollectionString.IndexOf(",");
				string cardType = cardCollectionString.Substring(1, commaIndex - 1);
				int cardInt = Int32.Parse(cardType);
				cardCollection.Add((eCard) cardInt);
				int closeBracketIndex = cardCollectionString.IndexOf(")");
				cardCollectionString = cardCollectionString.Substring(closeBracketIndex + 1);
			}
		}

		return cardCollection;
	}

	private void FillBoard(List<eCard> cardCollection = null)
	{
		List<eCard> allCards;
		// Fill the board with new cards
		if (cardCollection == null)
		{
			allCards = new List<eCard>();
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
			
		}
		// Fill the board with a given set of cards (load game)
		else
		{
			allCards = cardCollection;
		}
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				// Layout the cards on the board
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

	public void ResetChoices()
	{
		isFirstChoice = true;
	}

	public bool HasPlayerWon()
	{
		return requiredMatchesToWin == 0;
	}
}
