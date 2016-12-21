using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase
{
    public static class ExtensionMethodsFloat
    {
        #region IsEqual methods
        public static bool IsEqual(this float? obj1_in, float? obj2_in)
        {
            if (obj1_in == null || obj2_in == null)
            {
                if (obj1_in == null && obj2_in == null)
                    return true;
                else
                    return false;
            }

            return obj1_in.Value.IsEqual(obj2_in.Value, 6);
        }

        /// <summary>
        /// Compares 2 float, handled same if the difference is smaller than 0.0006% of larger obj
        /// </summary>
        /// <param name="obj1_in"></param>
        /// <param name="obj2_in"></param>
        /// <returns></returns>
        public static bool IsEqual(this float obj1_in, float obj2_in)
        {
            return obj1_in.IsEqual(obj2_in, 6);
        }

        /// <summary>
        /// Compares 2 float, handled same if the difference is smaller than maxPpmDiff_in PPM of larger parameter
        /// </summary>
        /// <param name="obj1_in"></param>
        /// <param name="obj2_in"></param>
        /// <param name="maxPpmDiff_in">max difference is maxPpmDiff_in part per million of larger parameter.</param>
        /// <returns></returns>
        public static bool IsEqual(this float obj1_in, float obj2_in, int maxPpmDiff_in)
        {
            float biggerAbs = Math.Max(Math.Abs(obj1_in), Math.Abs(obj2_in));
            if (biggerAbs == 0f)
            {
                return obj1_in == obj2_in;//both sould be zero
            }
            else
            {
                float epsilon = (maxPpmDiff_in * biggerAbs) / 1000000;

                return obj1_in.IsEqual(obj2_in, epsilon);
            }
        }

        /// <summary>
        /// Compares 2 float, handled same if the difference is smaller than maxDiff_in
        /// </summary>
        /// <param name="obj1_in"></param>
        /// <param name="obj2_in"></param>
        /// <returns></returns>
        public static bool IsEqual(this float obj1_in, float obj2_in, float maxDiff_in)
        {
            if ((obj1_in - maxDiff_in) > obj2_in)
                return false;
            if ((obj1_in + maxDiff_in) < obj2_in)
                return false;

            return true;
        }
        #endregion

        #region SI parser/ToString
        const int SI_PREFIX_STEP = 1000; // 1 step is 3 decade
        static char[] SI_PREFIX_BIGGER = new char[] {'k', 'M', 'G', 'T'};
        static char[] SI_PREFIX_SMALLER = new char[] { 'm', 'µ', 'n', 'p' };
        static IEnumerable<char> SI_PREFIX_ALL = SI_PREFIX_BIGGER.Union(SI_PREFIX_SMALLER);
                
        static public string ToSiStr(this float value_in )
        {
            int prefixOffset = 0;
            while (value_in >= SI_PREFIX_STEP)
            {
                value_in /= SI_PREFIX_STEP;
                prefixOffset++;
            }
            if (value_in == 0)
                return value_in.ToString("F");
            while (value_in < 1)
            {
                value_in *= SI_PREFIX_STEP;
                prefixOffset--;
            }

            return string.Format("{0} {1}", value_in.ToString(
                value_in != (int)value_in ? "F" : "F0"),
                GetSiPrefix(prefixOffset) ).TrimEnd();
        }

        static public float SiParse(this string string_in)
        {
            string_in = string_in.Trim();
            char decimalSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Trim()[0];

            IEnumerable<char> prefixChars = string_in.ToCharArray().Where(item => char.IsLetter(item)
                && item != decimalSeparator);
            
            int prefixOffset = 0;
            if (prefixChars.Count() > 0)
            {
                if (prefixChars.Count() > 1)
                    throw new ArgumentException(string.Format("more SI prefix character: {0}", prefixChars.ItemsToString()));

                char prefixChar = prefixChars.Single();
                for (int i = 0; i < SI_PREFIX_BIGGER.Length; i++)
                    if (SI_PREFIX_BIGGER[i] == prefixChar)
                        prefixOffset = i + 1;

                if(prefixOffset == 0)
                    for (int i = 0; i < SI_PREFIX_SMALLER.Length; i++)
                        if (SI_PREFIX_SMALLER[i] == prefixChar)
                            prefixOffset = -1 - i;

                if (prefixOffset == 0)
                {
                    if (prefixChar == 'u') // == 'µ'
                        prefixOffset = -2;
                    else
                        throw new ArgumentException(string.Format("SI prefix character {0} cannot be processed.", prefixChar));
                }

                if (string_in.Contains(decimalSeparator))
                    string_in = string_in.Remove(string_in.Length - 1); //remove last char: should be the Si prefix..
                else
                    string_in = string_in.Replace(prefixChar, decimalSeparator);
            }

            float res = float.Parse(string_in);
            res *= (float)Math.Pow((double)SI_PREFIX_STEP, (double)prefixOffset);
            
            return res;
        }

        static private char GetSiPrefix(int prefixOffset_in)
        {
            if (prefixOffset_in > 0)
                return SI_PREFIX_BIGGER[prefixOffset_in - 1];
            else if (prefixOffset_in < 0)
                return SI_PREFIX_SMALLER[~prefixOffset_in];
            return ' ';
        }
        #endregion

        static int MAX_FLOAT_EXPONENT = 40;
        public static float RoundToSignificantDecimals(this float value_in, uint numberOfSignificantDecimals_in)
        {
            int _sign = value_in.CompareTo(.0f);
            if (_sign == 0)
                return value_in;

            float _significant = value_in * _sign;
            int _exponentSign = _significant.CompareTo(1.0f);

            if (_exponentSign == 0)
                return value_in;

            if (_exponentSign == 1)
            {
                int _exp = 0;
                for (int i = 0; i < MAX_FLOAT_EXPONENT && _significant >= 10.0f; i++)
                {
                    _significant /= 10;
                    _exp = i;
                }
                _significant = _significant.RoundPosNormalToSignificantDecimals(numberOfSignificantDecimals_in);
                for (int i = 0; i < _exp; i++)
                {
                    _significant *= 10;
                }
                return _significant * _sign;
            }
            else
            {
                int _exp = 0;
                for (int i = 0; i < MAX_FLOAT_EXPONENT && _significant <= 1.0f; i++)
                {
                    _significant *= 10;
                    _exp = i;
                }
                _significant = _significant.RoundPosNormalToSignificantDecimals(numberOfSignificantDecimals_in);
                for (int i = 0; i < _exp; i++)
                {
                    _significant /= 10;
                }
                return _significant * _sign;
            }
        }
        private static float RoundPosNormalToSignificantDecimals(this float normaledValue_in, uint numberOfSignificantDecimals_in)
        {
            for (int i = 0; i < numberOfSignificantDecimals_in; i++)           
                normaledValue_in *= 10;

            normaledValue_in += 0.5f;
            normaledValue_in = (float)(int)normaledValue_in;

            for (int i = 0; i < numberOfSignificantDecimals_in; i++)
                normaledValue_in /= 10;

            return normaledValue_in;
        }
    }
}
