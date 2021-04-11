using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.SymbolStreamIO
{
    struct SymbolStream
    {
        public List<byte> Data;
        public int BitIndex;
        public int ByteIndex;
        public byte WorkingByte;
    }
}
