using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunnerLogics : MonoBehaviour
{
	private int width;
	private int height;
	public GameRunnerLogics(GameConfig gameConfig)
	{
		this.width = gameConfig.boardWidth;
		this.height = gameConfig.boardHeight;
	}
}
