﻿namespace Ude.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EUCJPContextAnalyser : JapaneseContextAnalyser
    {
        private const byte HIRAGANAFIRSTBYTE = 0xA4;

        protected override int GetOrder(byte[] buf, int offset, out int charLen)
        {
            byte high = buf[offset];

            // find out current char's byte length
            if (high == 0x8E || (high >= 0xA1 && high <= 0xFE))
            {
                charLen = 2;
            }
            else if (high == 0xBF)
            {
                charLen = 3;
            }
            else
            {
                charLen = 1;
            }

            // return its order if it is hiragana
            if (high == HIRAGANAFIRSTBYTE)
            {
                byte low = buf[offset + 1];
                if (low >= 0xA1 && low <= 0xF3)
                {
                    return low - 0xA1;
                }
            }

            return -1;
        }

        protected override int GetOrder(byte[] buf, int offset)
        {
            // We are only interested in Hiragana
            if (buf[offset] == HIRAGANAFIRSTBYTE)
            {
                byte low = buf[offset + 1];
                if (low >= 0xA1 && low <= 0xF3)
                {
                    return low - 0xA1;
                }
            }

            return -1;
        }
    }
}
