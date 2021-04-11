using AsymmetricNumeralSystemsTestPlatform.SymbolStreamIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.SymbolStreamIO
{
    class Reader
    {
        public SymbolStream SymbolStream;

        public Reader(List<byte> data)
        {
            SymbolStream.Data = data;
            SymbolStream.BitIndex = 0;
            SymbolStream.ByteIndex = 0;
            SymbolStream.WorkingByte = 0;
        }

        public bool HasData()
        {
            return SymbolStream.ByteIndex < SymbolStream.Data.Count;
        }

        public int GetSymbol()
        {
            int symbol = (SymbolStream.Data[SymbolStream.ByteIndex] >> SymbolStream.BitIndex) & 1;
            if (SymbolStream.BitIndex == 7)
            {
                SymbolStream.ByteIndex++;
                SymbolStream.BitIndex = 0;
            }
            else
            {
                SymbolStream.BitIndex++;
            }
            return symbol;
        }
    }
}
