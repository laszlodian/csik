using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using e77.MeasureBase;

namespace e77.MeasureBase.Extensions.Hex
{
    public static class ExtensionMethodsHex
    {
        static public String ToHex(this byte input)
        {
            /*/
            return input.ToString("X2");
            /*/
            String s = "";
            char hi = (char)((input >> 4) & 0x0F);
            char lo = (char)(input & 0x0F);
            ToHexChar(ref hi);
            ToHexChar(ref lo);
            s += hi;
            s += lo;
            return s;
            //*/
        }

        static public String ToHexString(this IEnumerable<byte> input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in input)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }



        static private void ToHexChar(ref char input)
        {
            if (0 <= input && input < 10)
            {
                input += (char)0x30;
            }
            else if (10 <= input && input < 16)
            {
                input += (char)(0x41 - 10);
            }
            else
                throw new InvalidCastException();
        }
    }
}
