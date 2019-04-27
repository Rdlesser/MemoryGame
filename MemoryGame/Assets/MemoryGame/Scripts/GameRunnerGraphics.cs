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

    public void InitializeEnvironment(eCard[,] board, string savedCardCollection = null)
    {
        if (cardCollection != null)
        {
            for (int i = 0; i < cardCollection.Count; i++)
            {
                Destroy(cardCollection[i]);
            }
        }

        // Use a tem instantiation point
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
                    // For each card space in our game - instantiate a card
                    GameObject cardObject = Instantiate(cardPrefab, cardInstantiationPoint,
                        Quaternion.identity, gameObject.transform);
                    cardObject.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnCardClick(cardObject);
                    });
                    Card cardScript = cardObject.GetComponent<Card>();
                    
                    // Set the card properties
                    cardScript.cardType = board[i, j];
                    cardScript.cardImage = GetImageForType(cardScript.cardType);
                    cardScript.isFlipped = false;
                    
                    Image cardImage = cardObject.GetComponent<Image>();
                    cardHeight = cardImage.sprite.rect.height;
                    cardWidth = cardImage.sprite.rect.width;
                    cardCollection.Add(cardObject);
                    
                    // Calculate the distance in Y axis - where we will instantiate the next card
                    float distanceY = cardHeight + cardMarginY;
                    Vector3 movementVector = new Vector3(0, - distanceY, 0);
                    cardInstantiationPoint += movementVector;
                }
    
                // Calculate the dinstance in X axis - where we will instantiate the next card
                float distanceX = cardWidth + cardMarginX;
                cardInstantiationPoint = new Vector3(cardInstantiationPoint.x + distanceX, 
                    boardInstantiationPointY, cardInstantiationPoint.z);
            }
        }
        // Case of using a given board (load game)
        else
        {
            bool[,] cardStates = ParseCardStatesFromString(board.GetLength(0),
                board.GetLength(1),
                savedCardCollection);
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    // For each card space in our game - instantiate a card
                    GameObject cardObject = Instantiate(cardPrefab, cardInstantiationPoint,
                        Quaternion.identity, gameObject.transform);
                    cardObject.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        OnCardClick(cardObject);
                    });
                    // Set the card properties accordig to our saved game
                    Card cardScript = cardObject.GetComponent<Card>();
                    cardScript.cardType = board[i, j];
                    cardScript.cardImage = GetImageForType(cardScript.cardType);
                    cardScript.isFlipped = cardStates[i, j];
                    
                    // In case the card was flipped in our saved game
                    if (cardStates[i, j])
                    {
                        cardObject.GetComponent<Image>().sprite = cardObject.GetComponent<Card>().cardImage;
                        cardObject.GetComponent<Button>().enabled = false;
                    }
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

        // Change the card properties
        card.GetComponent<Button>().enabled = false;
        card.GetComponent<Card>().isFlipped = true;

        PlayCardFlipAnimation(card);
        
        // Case this is the player's first card choice out of 2
        if (firstCardChoice == null)
        {
            firstCardChoice = card;
            
        }
        // Case this is the player's second card choice
        else
        {
            secondCardChoice = card;
            // Prevent user input as we would like to take a second and allow the user to look at the flipped cards
            AllowUserInput(false);
        }
        if (cardClickAction != null)
        {
            cardClickAction.Invoke(card.GetComponent<Card>().cardType);
        }
    }

    private void PlayCardFlipAnimation(GameObject card)
    {
        GoTweenChain tweenChain = new GoTweenChain ();
        
        ScaleTweenProperty tweenProperty = new ScaleTweenProperty (new Vector2(0f, 1f));
        GoTweenConfig tweenConfig = new GoTweenConfig();
        tweenConfig.addTweenProperty(tweenProperty);
        GoTween tween = new GoTween(card.transform, 0.25f, tweenConfig, (t)=>{
            card.GetComponent<Image>().sprite = card.GetComponent<Card>().cardImage;
        });
        tweenChain.append(tween);

        ScaleTweenProperty tweenBackProperty = new ScaleTweenProperty (Vector2.one, false);
        GoTweenConfig tweenBackConfig = new GoTweenConfig();
        tweenBackConfig.addTweenProperty(tweenBackProperty);
        GoTween tweenBack = new GoTween(card.transform, 0.25f, tweenBackConfig, (t)=>{
            
        });
        tweenChain.append(tweenBack);

        tweenChain.play ();
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

    /*
     * Returns a float representation of the time in the following format:
     * "minutes:[0]seconds"
     * such that for single digit seconds we add a preceding zero for cosmetic purposes
     */
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

    /*
     * Decrypt the string time to an int representation
     */
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

    /*
     * Clock Routine to be used for the timer
     */
    private IEnumerator ClockRoutine(float targetTime)
    {
        float timeLeft = targetTime;
        // Start count-down
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
            // Set the timer text
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
        FlipPlayerChoices();
		
        ResetChoices();
		
    }

    public void ResetChoices(bool enableUserInput = true)
    {
        firstCardChoice = null;
        secondCardChoice = null;

        AllowUserInput(enableUserInput);
    }

    /*
     * Flip the player choices over to their back side
     */
    public void FlipPlayerChoices()
    {
        if (firstCardChoice != null) 
        {
            firstCardChoice.GetComponent<Image>().sprite = cardBack;
            firstCardChoice.GetComponent<Button>().enabled = true;
            firstCardChoice.GetComponent<Card>().isFlipped = false;
            
            if (secondCardChoice != null) 
            {
                secondCardChoice.GetComponent<Image>().sprite = cardBack;
                secondCardChoice.GetComponent<Button>().enabled = true;
                secondCardChoice.GetComponent<Card>().isFlipped = false;
            }
        }

        
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
        
        // Remove all cards from the screen so that the main menu does not collide with the cards
        foreach (var card in cardCollection)
        {
            card.SetActive(false);
        }
    }

    /*
     * Returns the card collection in the following format:
     * "(card_type, is_flipped)"
     */
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

    public int GetRemainingTime()
    {
        return StringTimeToInt(timerText.text);
    }
}

[System.Serializable]
public class CardToImage
{
    public Sprite cardImage;
    public eCard cardType;
}