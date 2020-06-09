using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Poker;

public class MyDeck : MonoBehaviour
{
    List<PlayingCard> cards = new List<PlayingCard>();
    public PlayingCard popTopCard
    {
        get
        {
            PlayingCard ret = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return ret;
        }
    }
    public PlayingCard popRandomCard
    {
        get
        {
            int ranIndex = Random.Range(0, cards.Count);
            PlayingCard ret = cards[ranIndex];
            cards.RemoveAt(ranIndex);
            return ret;
        }
    }

    public void Reset()
    {
        cards = new List<PlayingCard>();
        for(int suit = 1; suit<=4; ++suit)
        {
            for(int face = 2; face <= 14; ++face)
            {
                PlayingCard newCard = new PlayingCard();
                newCard.face = (Face)face;
                newCard.suit = (Suit)suit;
                cards.Add(newCard);
            }
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int randIndex = Random.Range(i, cards.Count);
            PlayingCard temp = cards[i];
            cards[i] = cards[randIndex];
            cards[randIndex] = temp;
        }
    }

}
