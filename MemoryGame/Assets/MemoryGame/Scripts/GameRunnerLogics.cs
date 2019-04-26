using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameRunnerLogics : MonoBehaviour
{
	private int width;
	private int height;
	private eCard[,] board;
	private GameConfig gameConfig;
	
	public GameRunnerLogics(GameConfig gameConfig)
	{
		width = gameConfig.boardWidth;
		height = gameConfig.boardHeight;
		this.gameConfig = gameConfig;
		
		board = new eCard[width, height];
		FillBoard();
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
}
