using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace e77.MeasureBase
{
    public static class ExtensionMethodsUInt16
    {
        /// <summary>
        /// Higher byte of the integer
        /// </summary>
        /// <param name="input"></param>
        /// <returns>value of the higher byte</returns>
        static public byte Hi(this UInt16 input)
        {
            return (byte)(input >> 8);
        }
        /// <summary>
        /// Lower byte of the integer
        /// </summary>
        /// <param name="input"></param>
        /// <returns>value of the lower byte</returns>
        static public byte Lo(this UInt16 input)
        {
            return (byte)(input & 0x00FF);
        }
        /// <summary>
        /// returns the most significant bit of the 16 bit integer
        /// </summary>
        /// <param name="input"></param>
        /// <returns>false if MSB is 0; true if MSB is 1</returns>
        static public bool GetMSB(this UInt16 input)
        {
            return (input & 0x8000) == 0x8000;
        }
        /// <summary>
        /// Sets the most significant bit of the 16 bit integer
        /// </summary>
        /// <param name="output"></param>
        /// <param name="isSet">if it's true the MSB is set to 1 otherwise it's set to 0</param>
        static public void SetMSB(this UInt16 output, bool isSet)
        {
            if (isSet)
            {
                output |= 0x8000;
            }
            else
            {
                output &= 0x7FFF;
            }
        }

    }
    public static class ToUInt16
    {
      static public UInt16 SetWord(byte hi, byte lo)
      {
        return (UInt16)((hi << 8) | lo);
      }
    }
}
