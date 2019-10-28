using System;

namespace ShUtilities.Common
{
    public static class MathExtensions
    {
        /// <summary>
        /// Like <see cref="Math.Truncate(double)"/> but returns an <see cref="Int32"/>.
        /// </summary>
        /// <remarks>The method name is a hold-over from my Turbo Pascal/Delphi days.</remarks>
        public static int Trunc(this double d)
        {
            int result = (int)Math.Truncate(d);
            return result;
        }

        /// <summary>
        /// Returns only the fractional component of a <see cref="Double"/>. Suffers from
        /// problems with accurately representing real numbers in subtractions.
        /// </summary>
        /// <remarks>The method name is a hold-over from my Turbo Pascal/Delphi days.</remarks>
        public static double Frac(this double d)
        {
            double result = d - Math.Truncate(d);
            return result;
        }
    }
}
