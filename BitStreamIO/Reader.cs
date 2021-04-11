using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.BitStreamIO
{
    class Reader
    {
        public BitStream BitStream;

        public Reader(int endState, int endBitOffset, List<byte> data, long uncompressedLength)
        {
            BitStream.EndState = endState;
            BitStream.Data = data;
            BitStream.UncompressedLength = uncompressedLength;
            BitStream.BitIndex = endBitOffset == 0 ? 7 : endBitOffset - 1;
            BitStream.ByteIndex = data.Count - 1;
            BitStream.WorkingByte = 0;
        }
        public int GetBit()
        {
            int Bit = (BitStream.Data[BitStream.ByteIndex] >> BitStream.BitIndex) & 1;
            if (BitStream.BitIndex == 0)
            {
                BitStream.BitIndex = 7;
                BitStream.ByteIndex--;
            }
            else
            {
                BitStream.BitIndex--;
            }
            return Bit;
        }

        public bool HasBits()
        {
            return BitStream.ByteIndex >= 0;
        }
    }
}
