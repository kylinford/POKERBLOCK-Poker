using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class MyPlayer : MonoBehaviour
{
    public GameObject winnerIndicator;
    public Text textRank;
    public List<PlayingCard> cards { get; private set; }
    public List<MyCard> mycards;

    public Poker.Hand hand { get; private set; }

    public void Reset()
    {
        textRank.text = "";
        foreach (MyCard card in mycards)
        {
            card.Reset();
        }
    }

    public void UpdateContent(List<PlayingCard> cards)
    {
        if (cards.Count == mycards.Count)
        {
            this.cards = cards;
            UpdateContent();
        }
    }

    public void UpdateContent()
    {
        for (int i = 0; i < mycards.Count && i < mycards.Count; ++i)
        {
            mycards[i].UpdateContent(cards[i]);
        }

        //seven cards
        List<PlayingCard> sevenCards = new List<PlayingCard>(MyGameManager.singleton.pool.cards);
        foreach (PlayingCard card in cards)
        {
            sevenCards.Add(card);
        }

        //Update hand
        hand = new Poker.Hand(sevenCards); ;
        string rankString = Poker.Poker.rankDisplayName[hand.highestRank.Key];
        textRank.text = name + "\nHandRank: " + rankString;

    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder(name).Append(' ');
        sb.Append(hand.highestRank.Key).Append(' ');
        foreach(PlayingCard card in hand.highestRank.Value)
        {
            sb.Append(PlayingCard.faceDisplay[card.face]).Append(card.suit.ToString()[0]).Append(' ');
        }
        return sb.ToString();
    }
}
