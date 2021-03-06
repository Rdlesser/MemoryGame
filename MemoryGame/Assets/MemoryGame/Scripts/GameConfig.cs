﻿using System.Collections.Generic;
using UnityEngine;
/*
 * Game Configuration File
 */
public class GameConfig : MonoBehaviour
{

	public CardConfig cardConfig;
	public int boardWidth;
	public int boardHeight;
	public bool allowRepetitions = false;
	public int targetTime = 30;

	public bool shouldSaveDataPersistently;
}

[System.Serializable]
public class CardConfig
{
	public List<cardDefinition> cardDefinitions = new List<cardDefinition>();
}

[System.Serializable]
public class cardDefinition
{
	public eCard cardType;
	public Sprite cardImage;
	public int appearences;
}

public enum eCard
{
	CARD_1,
	CARD_2,
	CARD_3,
	CARD_4,
	CARD_5,
	CARD_6,
	CARD_7,
	CARD_8
}