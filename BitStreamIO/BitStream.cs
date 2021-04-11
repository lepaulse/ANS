using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.BitStreamIO
{
    public struct BitStream
    {
        public int EndState;
        public List<byte> Data;
        public long UncompressedLength;
        public int BitIndex;
        public int ByteIndex;
        public byte WorkingByte;
    }
}
