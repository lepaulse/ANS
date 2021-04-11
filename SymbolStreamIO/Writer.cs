using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.SymbolStreamIO
{
    class Writer
    {
        public SymbolStream SymbolStream;

        public Writer()
        {
            SymbolStream.Data = new List<byte>();
            SymbolStream.BitIndex = 7;
            //ByteIndex = 0;
            SymbolStream.WorkingByte = 0;
        }

        public void PutSymbol(int symbol)
        {
            SymbolStream.WorkingByte = (byte)(SymbolStream.WorkingByte ^ (symbol << SymbolStream.BitIndex));
            if (SymbolStream.BitIndex == 0)
            {
                SymbolStream.BitIndex = 7;
                SymbolStream.Data.Add(SymbolStream.WorkingByte);
                SymbolStream.WorkingByte = 0;
            }
            else
            {
                SymbolStream.BitIndex--;
            }
        }
        public void Reverse()
        {
            SymbolStream.Data.Reverse();
        }
    }
}
