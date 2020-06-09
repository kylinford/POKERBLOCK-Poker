using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPool : MonoBehaviour
{
    public List<PlayingCard> cards { get; private set; }
    public List<MyCard> mycards;

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
    }

    public void Reset()
    {
        foreach (MyCard card in mycards)
        {
            card.Reset();
        }
    }
}
