using System;
using System.Collections.Generic;

namespace ShUtilities.Text
{
    public readonly struct SingleByteString : IEquatable<SingleByteString>
    {
        internal SingleByteString(SingleByteStringStorageSegment segment, int start)
        {
            Segment = segment;
            Start = start;
        }

        internal readonly SingleByteStringStorageSegment Segment;
        internal readonly int Start;

        public int Length => Segment.GetStringLength(Start);

        // Combination with string
        // TODO: make more efficient by not allocating strings on each call

        public static string operator +(SingleByteString a, SingleByteString b) => a.ToString() + b.ToString();

        public static string operator +(string a, SingleByteString b) => a + b.ToString();

        public static string operator +(SingleByteString a, string b) => a.ToString() + b;

        public static implicit operator string(SingleByteString sbs) => sbs.ToString();

        public override string ToString() => Segment.GetString(Start);

        // Equality

        public static bool operator ==(SingleByteString left, SingleByteString right) => left.Equals(right);

        public static bool operator !=(SingleByteString left, SingleByteString right) => !(left == right);

        public override bool Equals(object obj) => obj is SingleByteString sbs && Equals(sbs);

        public bool Equals(SingleByteString other) => SingleByteStringStorageSegment.GetStringEquals(Segment, Start, other.Segment, other.Start);

        public override int GetHashCode() => Segment.GetStringHashCode(Start);
    }
}
