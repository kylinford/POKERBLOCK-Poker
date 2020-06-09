using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class PlayingCard : Comparer<PlayingCard>, IComparable<PlayingCard>, IEquatable<PlayingCard>
{
    public Suit suit;
    public Face face;

    public static Dictionary<Face, string> faceDisplay = new Dictionary<Face, string>()
    {
        { Face.Ace, "A"},
        { Face.Two, "2"},
        { Face.Three, "3"},
        { Face.Four, "4"},
        { Face.Five, "5"},
        { Face.Six, "6"},
        { Face.Seven, "7"},
        { Face.Eight, "8"},
        { Face.Nine, "9"},
        { Face.Ten, "10"},
        { Face.Jack, "J"},
        { Face.Queen, "Q"},
        { Face.King, "K"},
    };

    public override int Compare(PlayingCard x, PlayingCard y)
    {
        return x.CompareTo(y);
    }

    public int CompareTo(PlayingCard other)
    {
        return other.face - face;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as PlayingCard);
    }

    public bool Equals(PlayingCard other)
    {
        return other != null &&
               suit == other.suit &&
               face == other.face;
    }

    public override int GetHashCode()
    {
        int hashCode = 1731198300;
        hashCode = hashCode * -1521134295 + suit.GetHashCode();
        hashCode = hashCode * -1521134295 + face.GetHashCode();
        return hashCode;
    }

    public static bool operator ==(PlayingCard left, PlayingCard right)
    {
        return EqualityComparer<PlayingCard>.Default.Equals(left, right);
    }

    public static bool operator !=(PlayingCard left, PlayingCard right)
    {
        return !(left == right);
    }
}

public enum Suit { Unknown, Diamond, Club, Heart, Spade };
public enum Face { Unknown, Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }

