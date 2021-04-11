using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.BitStreamIO
{
    class Writer
    {
        public BitStream BitStream;
        public Writer()
        {
            BitStream.EndState = -1;
            BitStream.Data = new List<byte>();
            BitStream.UncompressedLength = -1;
            BitStream.BitIndex = 0;
            BitStream.ByteIndex = 0;
            BitStream.WorkingByte = 0;
        }

        public void PutBit(int bit)
        {
            BitStream.WorkingByte = (byte)(BitStream.WorkingByte ^ (bit << BitStream.BitIndex));
            if (BitStream.BitIndex == 7)
            {
                BitStream.BitIndex = 0;
                BitStream.Data.Add(BitStream.WorkingByte);
                BitStream.WorkingByte = 0;
            }
            else
            {
                BitStream.BitIndex++;
            }
        }

        public void Terminate(long uncompressedLength, int endState)
        {
            if (BitStream.BitIndex != 0)
            {
                BitStream.Data.Add(BitStream.WorkingByte);
            }
            BitStream.UncompressedLength = uncompressedLength;
            BitStream.EndState = endState;
        }
    }
}
