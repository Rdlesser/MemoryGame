using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameRunnerGraphics: MonoBehaviour
{

    public Action OnTimerEnded;
    public Action<eCard> cardClickAction;

    [SerializeField] private Vector3 boardInstantiationPoint;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private float cardMarginX;
    [SerializeField] private float cardMarginY;
    [SerializeField] private Text timerText;
    [SerializeField] private List<CardToImage> cardToImageList;
    [SerializeField] private Sprite cardBack;

    private bool isUserInputAllowed = true;
    private bool finished = false;
    private float cardHeight;
    private float cardWidth;

    private GameObject firstCardChoice;
    private GameObject secondCardChoice;

    private List<GameObject> cardCollection;

    private Coroutine timerRoutine;

    public void InitializeEnvironment(eCard[,] board, string savedCardCollection = null, int time = -1)
    {
        if (cardCollection != null)
        {
            for (int i = 0; i < cardCollection.Count; i++)
            {
                Destroy(cardCollection[i]);
            }
        }

        Vector3 cardInstantiationPoint = boardInstantiationPoint;
        float boardInstantiationPointY = cardInstantiationPoint.y;
        cardCollection = new List<GameObject>();
        
        // Instantiation of the board
        if (savedCardCollection == null)
        {
            
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    GameObject cardObject = Instantiate(cardPrefab, cardInstantiationPoint,
                        Quaternion.identity, gameObject.transform);
                    cardObject.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnCardClick(cardObject);
                    });
                    Card cardScript = cardObject.GetComponent<Card>();
                    cardScript.cardType = board[i, j];
                    cardScript.cardImage = GetImageForType(cardScript.cardType);
                    cardScript.isFlipped = false;
                    Image cardImage = cardObject.GetComponent<Image>();
                    cardHeight = cardImage.sprite.rect.height;
                    cardWidth = cardImage.sprite.rect.width;
                    cardCollection.Add(cardObject);
                    float distanceY = cardHeight + cardMarginY;
                    Vector3 movementVector = new Vector3(0, - distanceY, 0);
                    cardInstantiationPoint += movementVector;
                }
    
                float distanceX = cardWidth + cardMarginX;
                cardInstantiationPoint = new Vector3(cardInstantiationPoint.x + distanceX, 
                    boardInstantiationPointY, cardInstantiationPoint.z);
            }
        }
        else
        {
            bool[,] cardStates = ParseCardStatesFromString(board.GetLength(0),
                board.GetLength(1),
                savedCardCollection);
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    GameObject cardObject = Instantiate(cardPrefab, cardInstantiationPoint,
                        Quaternion.identity, gameObject.transform);
                    cardObject.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnCardClick(cardObject);
                    });
                    Card cardScript = cardObject.GetComponent<Card>();
                    cardScript.cardType = board[i, j];
                    cardScript.cardImage = GetImageForType(cardScript.cardType);
                    cardScript.isFlipped = cardStates[i, j];
                    if (cardStates[i, j])
                    {
                        cardObject.GetComponent<Image>().sprite = cardObject.GetComponent<Card>().cardImage;
                        cardObject.GetComponent<Button>().enabled = false;
                    }
                    
                    float distanceY = cardHeight + cardMarginY;
                    Vector3 movementVector = new Vector3(0, - distanceY, 0);
                    cardInstantiationPoint += movementVector;
                }
                float distanceX = cardWidth + cardMarginX;
                cardInstantiationPoint = new Vector3(cardInstantiationPoint.x + distanceX, 
                    boardInstantiationPointY, cardInstantiationPoint.z);

            }
        }
    }

    private bool[,] ParseCardStatesFromString(int boardWidth, int boardLength, string cardcollection)
    {
        bool[,] cardCollectionArray = null;
        if (cardcollection != null)
        {
            cardCollectionArray = new bool[boardWidth, boardLength];
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardLength; j++)
                {
                    int commaIndex = cardcollection.IndexOf(",");
                    int closeBracketIndex = cardcollection.IndexOf(")");
                    string isFlippedString = cardcollection.Substring(commaIndex + 1,
                        closeBracketIndex - commaIndex - 1);
                    bool isFlipped = isFlippedString == "True";
                    cardCollectionArray[i, j] = isFlipped;
                    cardcollection = cardcollection.Substring(closeBracketIndex + 1);
                }
            }
        }

        return cardCollectionArray;
    }

    private Sprite GetImageForType(eCard cardScriptCardType)
    {
        foreach (var cardImagePair in cardToImageList)
        {
            if (cardImagePair.cardType == cardScriptCardType)
            {
                return cardImagePair.cardImage;
            }			
        }

        return null;
    }

    public void OnCardClick(GameObject card)
    {
        if (!isUserInputAllowed)
        {
            return;
        }

        // TODO: call card script to be added to the prefab.
        // this will call card.getType and return it to the gamemanager.
        // TODO: card click animation
        // TODO: change sprite to appropriate sprite
        // TODO: play card flip sound
        card.GetComponent<Button>().enabled = false;
        card.GetComponent<Image>().sprite = card.GetComponent<Card>().cardImage;
        card.GetComponent<Card>().isFlipped = true;
        if (firstCardChoice == null)
        {
            firstCardChoice = card;
            
        }
        else
        {
            secondCardChoice = card;
            AllowUserInput(false);
        }
        if (cardClickAction != null)
        {
            cardClickAction.Invoke(card.GetComponent<Card>().cardType);
        }
    }

    public void StartClock(float targetTime)
    {
        timerText.gameObject.SetActive(true);
        timerText.text = FloatTimeToString(targetTime);
        timerRoutine = StartCoroutine(ClockRoutine(targetTime));
    }

    public int StopClock()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
        }

        return StringTimeToInt(timerText.text);
    }

    private string FloatTimeToString(float time)
    {
        string minutes = ((int) time / 60).ToString();
        float secondsLeft = time % 60;
        string seconds = secondsLeft.ToString("N0");

        if (secondsLeft < 10)
        {
            seconds = "0" + seconds;
        }
        
        return minutes + ":" + seconds;
    }

    private int StringTimeToInt(string time)
    {
        int colonIndex = time.IndexOf(":");
        string minutes = time.Substring(0, colonIndex);
        string seconds = time.Substring(colonIndex + 1);
        int minutesInt = Int32.Parse(minutes);
        minutesInt *= 60;
        int secondsInt = Int32.Parse(seconds);
        int timeRemaining = minutesInt + secondsInt;
        return timeRemaining;

    }

    private IEnumerator ClockRoutine(float targetTime)
    {
        float timeLeft = targetTime;
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            timerText.text = FloatTimeToString(timeLeft);
        }

        if (OnTimerEnded != null)
        {
            OnTimerEnded.Invoke();
        }
		
    } 

    public void OnMatchFailed()
    {
        StartCoroutine(OnMatchFailedRoutine());
    }

    IEnumerator OnMatchFailedRoutine()
    {
        yield return new WaitForSeconds(1.2f);
        firstCardChoice.GetComponent<Image>().sprite = cardBack;
        firstCardChoice.GetComponent<Button>().enabled = true;
        firstCardChoice.GetComponent<Card>().isFlipped = false;
        
        secondCardChoice.GetComponent<Image>().sprite = cardBack;
        secondCardChoice.GetComponent<Button>().enabled = true;
        secondCardChoice.GetComponent<Card>().isFlipped = false;
		
        ResetChoices();
		
    }

    public void ResetChoices()
    {
        firstCardChoice = null;
        secondCardChoice = null;

        AllowUserInput(true);
    }

    public void AllowUserInput(bool allow)
    {
        isUserInputAllowed = allow;
    }

    public void OnGameEnd(bool hasWon)
    {
        if (hasWon)
        {
            StopClock();
        }
        timerText.gameObject.SetActive(false);
        foreach (var card in cardCollection)
        {
            card.SetActive(false);
        }
    }

    public string GetCardCollectionAsString()
    {
        string ans = "";
        foreach (var card in cardCollection)
        {
            ans += "(";
            
            Card cardScript = card.GetComponent<Card>();
            int cardType = (int) cardScript.cardType;
            bool isFlipped = cardScript.isFlipped;

            ans += cardType + "," + isFlipped;

            ans += ")";
        }

        return ans;
    }
}

[System.Serializable]
public class CardToImage
{
    public Sprite cardImage;
    public eCard cardType;
}