﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker
{
    public enum Rank
    {
        HighCard = 1, OnePair, TwoPair, ThreeOfAKind, Straight,
        Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush
    }

    public class Poker
    {
        public static Dictionary<Rank, string> rankDisplayName = new Dictionary<Rank, string>()
        {
            { Rank.Flush, "Flush" },
            { Rank.FourOfAKind, "Four Of A Kind" },
            { Rank.FullHouse, "Full House" },
            { Rank.HighCard, "High Card" },
            { Rank.OnePair, "One Pair" },
            { Rank.RoyalFlush, "Royal Flush" },
            { Rank.Straight, "Straight" },
            { Rank.StraightFlush, "Straight Flush" },
            { Rank.ThreeOfAKind, "Three Of A Kind" },
            { Rank.TwoPair, "Two Pair" }
        };
    }

    public class Hand : Comparer<Hand>, IComparable<Hand>, IEquatable<Hand>
    {
        public List<PlayingCard> cards_Descending { get; private set; }
        public SortedDictionary<Rank, List<PlayingCard>> ranks = new SortedDictionary<Rank, List<PlayingCard>>();
        public KeyValuePair<Rank, List<PlayingCard>> highestRank { get { return ranks.Last(); } }

        public Hand (List<PlayingCard> cards)
        {
            cards_Descending = new List<PlayingCard>(cards.OrderByDescending(x => x.face).ToList());
            GetRank();
        }

        private void GetRank()
        {
            //Analyse hand
            List<PlayingCard> consecutiveFace = new List<PlayingCard>();
            SortedDictionary<Face, int> faceCount = new SortedDictionary<Face, int>();
            Dictionary<Suit, int> suitCount = new Dictionary<Suit, int>();
            Face fourOfAKindFace = Face.Unknown;
            Suit flushSuit = Suit.Unknown;
            for(int i = 0; i < cards_Descending.Count; ++i)
            {
                PlayingCard curr = cards_Descending[i];

                //consecutiveFace
                if (consecutiveFace.Count != 0 &&
                    consecutiveFace[consecutiveFace.Count - 1].face - curr.face > 1)
                {
                    consecutiveFace.Clear();
                }
                if (consecutiveFace.Count == 0
                    || consecutiveFace[consecutiveFace.Count - 1].face != curr.face)
                {
                    consecutiveFace.Add(curr);
                }

                //faceCount
                if (!faceCount.ContainsKey(curr.face))
                {
                    faceCount.Add(curr.face, 0);
                }
                ++faceCount[curr.face];
                if (faceCount[curr.face] == 4)
                {
                    fourOfAKindFace = curr.face;
                }

                //suitCount
                if (!suitCount.ContainsKey(curr.suit))
                {
                    suitCount.Add(curr.suit, 0);
                }
                ++suitCount[curr.suit];
                if (suitCount[curr.suit] == 5)
                {
                    flushSuit = curr.suit;
                }
            }
            faceCount.Reverse();

            //High card
            ranks.Add(Rank.HighCard, cards_Descending);
            //Straight, Straight Flush, Royal Straight
            if (consecutiveFace.Count >= 5)
            {
                //Straight
                ranks.Add(Rank.Straight, consecutiveFace);
                int indexFlushStart = 0;

                //Straight Flush
                for (int indexFlushEnd = 1; indexFlushEnd < consecutiveFace.Count; ++indexFlushEnd)
                {
                    if (consecutiveFace[indexFlushEnd].suit != consecutiveFace[indexFlushStart].suit)
                    {
                        indexFlushStart = indexFlushEnd;
                        if (indexFlushStart > 2)
                        {
                            break;
                        }
                    }

                    if (indexFlushEnd - indexFlushStart + 1 == 5)
                    {
                        List<PlayingCard> det = new List<PlayingCard>();
                        for (int j = indexFlushStart; j <= indexFlushEnd; ++j)
                        {
                            det.Add(consecutiveFace[j]);
                        }
                        ranks.Add(Rank.StraightFlush, det);

                        //Royal Straight
                        if (det[0].face == Face.Ace)
                        {
                            ranks.Add(Rank.RoyalFlush, det);
                        }
                        break;
                    }
                }
            }
            //Four Of A Kind
            if (fourOfAKindFace != Face.Unknown)
            {
                List<PlayingCard> det = new List<PlayingCard>();
                foreach (PlayingCard card in cards_Descending)
                {
                    if (card.face == fourOfAKindFace)
                    {
                        det.Add(card);
                        break;
                    }
                }
                ranks.Add(Rank.FourOfAKind, det);
            }
            //Flush
            if (flushSuit != Suit.Unknown)
            {
                List<PlayingCard> det = new List<PlayingCard>();
                foreach (PlayingCard card in cards_Descending)
                {
                    if (card.suit == flushSuit)
                    {
                        det.Add(card);
                    }
                }
                ranks.Add(Rank.Flush, det);
            }
            //Full House, Three Of A Kind, One Pair, Two Pairs
            PlayingCard cardHighestThree = null;
            PlayingCard cardHighestPair = null;
            PlayingCard cardSecondHighestPair = null;
            foreach (Face currFace in faceCount.Keys)
            {
                if (faceCount[currFace] == 3)
                {
                    if (cardHighestThree == null)
                    {
                        foreach(PlayingCard card in cards_Descending)
                        {
                            if (card.face == currFace)
                            {
                                cardHighestThree = card;
                                break;
                            }
                        }
                    }
                }
                if (faceCount[currFace] == 2)
                {
                    if (cardHighestPair == null)
                    {
                        foreach (PlayingCard card in cards_Descending)
                        {
                            if (card.face == currFace)
                            {
                                cardHighestPair = card;
                                break;
                            }
                        }
                    }
                    else if (cardSecondHighestPair == null)
                    {
                        foreach (PlayingCard card in cards_Descending)
                        {
                            if (card.face == currFace)
                            {
                                cardSecondHighestPair = card;
                                break;
                            }
                        }
                    }
                }
            }
            if (cardHighestThree != null && cardHighestPair != null)
            {
                //Full House
                ranks.Add(Rank.FullHouse, new List<PlayingCard>() { cardHighestThree, cardHighestPair });
            }
            else if (cardHighestThree != null)
            {
                //Three of a kind
                List<PlayingCard> det = new List<PlayingCard>();
                det.Add(cardHighestThree);
                foreach (PlayingCard card in cards_Descending)
                {
                    if (card.face != cardHighestThree.face)
                    {
                        det.Add(card);
                        if (det.Count == 3)
                        {
                            break;
                        }
                    }
                }
                ranks.Add(Rank.ThreeOfAKind, det);

            }
            else if (cardHighestPair != null && cardSecondHighestPair != null)
            {
                //Two Pairs
                List<PlayingCard> det = new List<PlayingCard>() { cardHighestPair, cardSecondHighestPair };
                foreach (PlayingCard card in cards_Descending)
                {
                    if (card.face != cardHighestPair.face && card.face != cardSecondHighestPair.face)
                    {
                        det.Add(card);
                        break;
                    }
                }
                ranks.Add(Rank.TwoPair, det);
            }
            else if (cardHighestPair != null)
            {
                //One Pair
                List<PlayingCard> det = new List<PlayingCard>() { cardHighestPair };
                foreach (PlayingCard card in cards_Descending)
                {
                    if (card.face != cardHighestPair.face)
                    {
                        det.Add(card);
                        if (det.Count == 4)
                        {
                            break;
                        }
                    }
                }
                ranks.Add(Rank.OnePair, det);
            }

        }

        public override int Compare(Hand x, Hand y)
        {
            if (x.highestRank.Key != y.highestRank.Key)
            {
                return x.highestRank.Key - y.highestRank.Key;
            }
            else
            {
                int diffThree = 0;
                int diffHighestPair = 0;
                int diffSecondHighestPair = 0;
                int diffHighestSingle = 0;
                int diffSecondHighestSingle = 0;
                int diffThirdHighestSingle = 0;
                switch (x.highestRank.Key)
                {

                    //case Rank.RoyalFlush:
                    //case Rank.StraightFlush:
                    //case Rank.FourOfAKind:
                    //case Rank.Flush:
                    //case Rank.Straight:
                    default:
                        return x.highestRank.Value[0].face - y.highestRank.Value[0].face;
                    case Rank.FullHouse:
                        diffThree = x.highestRank.Value[0].face - y.highestRank.Value[0].face;
                        diffHighestPair = x.highestRank.Value[1].face - y.highestRank.Value[1].face;
                        return diffThree != 0 ? diffThree : diffHighestPair;
                    case Rank.ThreeOfAKind:
                        diffThree = x.highestRank.Value[0].face - y.highestRank.Value[0].face;
                        diffHighestSingle = x.highestRank.Value[1].face - y.highestRank.Value[1].face;
                        diffSecondHighestSingle = x.highestRank.Value[2].face - y.highestRank.Value[2].face;
                        if (diffThree != 0)
                        {
                            return diffThree;
                        }
                        else if (diffHighestSingle != 0)
                        {
                            return diffHighestSingle;
                        }
                        else
                        {
                            return diffSecondHighestSingle;
                        }
                    case Rank.TwoPair:
                        diffHighestPair = x.highestRank.Value[0].face - y.highestRank.Value[0].face;
                        diffSecondHighestPair = x.highestRank.Value[1].face - y.highestRank.Value[1].face;
                        diffHighestSingle = x.highestRank.Value[2].face - y.highestRank.Value[2].face;
                        if (diffHighestPair != 0)
                        {
                            return diffHighestPair;
                        }
                        else if (diffSecondHighestPair != 0)
                        {
                            return diffSecondHighestPair;
                        }
                        else
                        {
                            return diffHighestSingle;
                        }
                    case Rank.OnePair:
                        diffHighestPair = x.highestRank.Value[0].face - y.highestRank.Value[0].face;
                        diffHighestSingle = x.highestRank.Value[1].face - y.highestRank.Value[1].face;
                        diffSecondHighestSingle = x.highestRank.Value[2].face - y.highestRank.Value[2].face;
                        diffThirdHighestSingle = x.highestRank.Value[3].face - y.highestRank.Value[3].face;
                        if (diffHighestPair != 0)
                        {
                            return diffHighestPair;
                        }
                        else if (diffHighestSingle != 0)
                        {
                            return diffHighestSingle;
                        }
                        else if (diffSecondHighestSingle != 0)
                        {
                            return diffSecondHighestSingle;
                        }
                        else
                        {
                            return diffThirdHighestSingle;
                        }
                    case Rank.HighCard:
                        for(int i=0; i<5; ++i)
                        {
                            int diff = x.highestRank.Value[i].face - y.highestRank.Value[i].face;
                            if (diff != 0)
                            {
                                return diff;
                            }
                        }
                        return 0;
                }
            }
        }

        public int CompareTo(Hand other)
        {
            return Compare(this, other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Hand);
        }

        public bool Equals(Hand other)
        {
            return other != null && Compare(this, other) == 0;
        }

        public override int GetHashCode()
        {
            return 1920351605 + highestRank.GetHashCode();
        }

        public static bool operator ==(Hand left, Hand right)
        {
            return EqualityComparer<Hand>.Default.Equals(left, right);
        }

        public static bool operator !=(Hand left, Hand right)
        {
            return !(left == right);
        }

        public static bool operator > (Hand left, Hand right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator < (Hand left, Hand right)
        {
            return left.CompareTo(right) < 0;
        }

    }
}



