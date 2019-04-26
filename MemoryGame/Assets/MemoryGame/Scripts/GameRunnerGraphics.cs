using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRunnerGraphics: MonoBehaviour
{

	[SerializeField] private Vector3 boardInstantiationPoint;
	[SerializeField] private GameObject cardPrefab;
	[SerializeField] private float cardMarginX;
	[SerializeField] private float cardMarginY;

	private float cardHeight;
	private float cardWidth;

	public void InitializedEnvironment(eCard[,] board)
	{
		float boardInstantiationPointY = boardInstantiationPoint.y;
		for (int i = 0; i < board.GetLength(0); i++)
		{
			for (int j = 0; j < board.GetLength(1); j++)
			{
				GameObject cardObject = Instantiate(cardPrefab, boardInstantiationPoint, Quaternion.identity, gameObject.transform);
				cardObject.GetComponent<Button>().onClick.AddListener(() =>
				{
					OnCardClick(cardObject);
				});
				cardHeight = cardObject.GetComponent<Image>().sprite.rect.height;
				cardWidth = cardObject.GetComponent<Image>().sprite.rect.width;
				float distanceY = cardHeight + cardMarginY;
				Vector3 movementVector = new Vector3(0, - distanceY, 0);
				boardInstantiationPoint += movementVector;
			}

			float distanceX = cardWidth + cardMarginX;
			boardInstantiationPoint = new Vector3(boardInstantiationPoint.x + distanceX, boardInstantiationPointY, boardInstantiationPoint.z);
		}
	}

	public void OnCardClick(GameObject card)
	{
		// TODO: call card script to be added to the prefab.
		// this will call card.getType and return it to the gamemanager.
		card.gameObject.SetActive((false));
		Debug.LogError("Card Clicked!");
	}
}
