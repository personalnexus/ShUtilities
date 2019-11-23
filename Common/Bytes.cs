﻿using System;
using System.Globalization;

namespace ShUtilities.Common
{
    /// <summary>
    /// Working with counts of bytes at different scales in a way inspired by <see cref="TimeSpan"/>
    /// </summary>
    public struct Bytes : IEquatable<Bytes>
    {
        private struct ThresholdUnitName
        {
            public ThresholdUnitName(ulong threshold, string name)
            {
                Threshold = threshold;
                Name = name;
            }

            internal ulong Threshold { get; }
            internal string Name { get; }
        }

        private static readonly ThresholdUnitName[] Scales =
        {
            new ThresholdUnitName(0, "B"),
            new ThresholdUnitName(Kilo, "KB"),
            new ThresholdUnitName(Mega, "MB"),
            new ThresholdUnitName(Giga, "GB"),
            new ThresholdUnitName(Tera, "TB"),
            new ThresholdUnitName(Peta, "PB"),
            new ThresholdUnitName(Exa, "EB"),
        };

        public const ulong Kilo = 1024;
        public const ulong Mega = 1024 * Kilo;
        public const ulong Giga = 1024 * Mega;
        public const ulong Tera = 1024 * Giga;
        public const ulong Peta = 1024 * Tera;
        public const ulong Exa = 1024 * Peta;

        // Constructor + factory methods

        public Bytes(double count)
        {
            TotalBytes = (ulong)Math.Round(count, 0);
        }

        public Bytes(ulong count)
        {
            TotalBytes = count;
        }

        public static Bytes FromKilo(double count) => new Bytes(checked(count * Kilo));
        public static Bytes FromMega(double count) => new Bytes(checked(count * Mega));
        public static Bytes FromGiga(double count) => new Bytes(checked(count * Giga));
        public static Bytes FromTera(double count) => new Bytes(checked(count * Tera));
        public static Bytes FromPeta(double count) => new Bytes(checked(count * Peta));
        public static Bytes FromExa(double count)  => new Bytes(checked(count * Exa));

        public static Bytes FromKilo(ulong count) => new Bytes(checked(count * Kilo));
        public static Bytes FromMega(ulong count) => new Bytes(checked(count * Mega));
        public static Bytes FromGiga(ulong count) => new Bytes(checked(count * Giga));
        public static Bytes FromTera(ulong count) => new Bytes(checked(count * Tera));
        public static Bytes FromPeta(ulong count) => new Bytes(checked(count * Peta));
        public static Bytes FromExa(ulong count)  => new Bytes(checked(count * Exa));

        // At different scales

        public ulong TotalBytes { get; }
        public double TotalKilo => (double)TotalBytes / Kilo;
        public double TotalMega => (double)TotalBytes / Mega;
        public double TotalGiga => (double)TotalBytes / Giga;
        public double TotalTera => (double)TotalBytes / Tera;
        public double TotalPeta => (double)TotalBytes / Peta;
        public double TotalExa  => (double)TotalBytes / Exa;

        // Math operators

        public static Bytes operator +(Bytes value1, Bytes value2) => new Bytes(value1.TotalBytes + value2.TotalBytes);
        public static Bytes operator -(Bytes value1, Bytes value2) => new Bytes(value1.TotalBytes - value2.TotalBytes);
        public static Bytes operator *(Bytes value, ulong multiplier) => new Bytes(value.TotalBytes * multiplier);
        public static Bytes operator *(ulong multiplier, Bytes value) => new Bytes(value.TotalBytes * multiplier);

        public static implicit operator int(Bytes value) => (int)value.TotalBytes;
        public static implicit operator uint(Bytes value) => (uint)value.TotalBytes;
        public static implicit operator long(Bytes value) => (long)value.TotalBytes;
        public static implicit operator ulong(Bytes value) => value.TotalBytes;

        // Equality

        public static bool operator ==(Bytes bytes1, Bytes bytes2) => bytes1.Equals(bytes2);
        public static bool operator !=(Bytes bytes1, Bytes bytes2) => !bytes1.Equals(bytes2);
        public override bool Equals(object obj) => obj is Bytes && Equals((Bytes)obj);
        public bool Equals(Bytes other) => TotalBytes == other.TotalBytes;
        public override int GetHashCode() => TotalBytes.GetHashCode();

        // Formatting

        public override string ToString() => Format(TotalBytes);

        public string ToString(IFormatProvider formatProvider) => Format(TotalBytes, formatProvider);

        public static string Format(ulong byteCount) => Format(byteCount, NumberFormatInfo.InvariantInfo);

        public static string Format(ulong byteCount, IFormatProvider provider)
        {
            string result;
            // Don't get into decimals when we have less than 1KB
            if (byteCount < 1024)
            {
                result =  byteCount + "B";
            }
            else
            {
                ref ThresholdUnitName thresholdUnitName = ref Scales[Scales.Length - 1];
                for (int i = 1; i < Scales.Length; i++)
                {
                    if (Scales[i].Threshold > byteCount)
                    {
                        thresholdUnitName = ref Scales[i - 1];
                        break;
                    }
                }
                decimal byteCountInUnit = Math.Round((decimal)byteCount / thresholdUnitName.Threshold, 2);
                result = string.Format(provider, "{0}{1}", byteCountInUnit, thresholdUnitName.Name);
            }
            return result;
        }
    }
}