using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyGameManager : MonoBehaviour
{
    public MyDeck deck;
    public MyPlayer player1;
    public MyPlayer player2;
    public MyPool pool;
    public MyPlayer winner { get; private set; }
    public Text textTie;

    public static MyGameManager singleton;

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this); 
        }
    }
    private void Start()
    {
        PlayAgain();
    }

    public void PlayAgain()
    {
        deck.Reset();
        deck.Shuffle();
        List<PlayingCard> cardsForPlayer1 = new List<PlayingCard>();
        List<PlayingCard> cardsForPlayer2 = new List<PlayingCard>();
        List<PlayingCard> cardsForPool = new List<PlayingCard>();
        for(int i=0;i<2;++i)
        {
            cardsForPlayer1.Add(deck.popTopCard);
            cardsForPlayer2.Add(deck.popTopCard);
        }
        for (int i = 0; i < 5; ++i)
        {
            cardsForPool.Add(deck.popTopCard);
        }

        pool.UpdateContent(cardsForPool);
        player1.UpdateContent(cardsForPlayer1);
        player2.UpdateContent(cardsForPlayer2);

        if (player1.hand > player2.hand)
        {
            winner = player1;
        }
        else if (player1.hand < player2.hand)
        {
            winner = player2;
        }
        else
        {
            //tie
            winner = null;
        }
        player1.winnerIndicator.SetActive(false);
        player2.winnerIndicator.SetActive(false);
        textTie.gameObject.SetActive(false);
        if (winner != null)
        {
            winner.winnerIndicator.SetActive(true);
        }
        else
        {
            textTie.gameObject.SetActive(true);
        }
        
        Debug.Log(player1);
        Debug.Log(player2);
        Debug.Log(winner == null ? "tie" : winner.name);
        Debug.Log("\n");
    }
}
