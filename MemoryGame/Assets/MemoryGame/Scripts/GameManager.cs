using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

	private GameRunnerLogics gameRunnerLogics;
	[SerializeField] private GameRunnerGraphics gameRunnerGraphics;
	[SerializeField] private GameConfig gameConfig;
	
	private void Awake()
	{
		gameRunnerLogics = new GameRunnerLogics(gameConfig);
	}
}
