﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A class representing a card
 */
public class Card : MonoBehaviour
{

	public eCard cardType { get; set; }
	public Sprite cardImage { get; set; }
	
	public bool isFlipped { get; set; }
}
