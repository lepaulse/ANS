using AsymmetricNumeralSystemsTestPlatform.SymbolStreamIO;
using AsymmetricNumeralSystemsTestPlatform.BitStreamIO;
using AsymmetricNumeralSystemsTestPlatform.StaticProbabilityANS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AsymmetricNumeralSystemsTestPlatform.FileIO
{
    class Writer
    {
        public readonly int[] Alphabet = { 0, 1 };
        public readonly int[] FrequencyCounts = { 224, 32 };

        public void StreamEncodeFile(string inputFile, string outputFile)
        {
            var Encoder = new StaticProbabilityANS.Encoder(Alphabet, FrequencyCounts);
            var InputFile = new BinaryReader(File.Open(inputFile, FileMode.Open));
            List<byte> input = new List<byte>();
            for (int i = 0; i < InputFile.BaseStream.Length; i++)
            {
                input.Add(InputFile.ReadByte());
            }
            BitStream encodedChunk = Encoder.EncodeANSBitStream(input);
            var OutputFile = new BinaryWriter(File.Open(outputFile, FileMode.Create));
            // TODO: Optimize metadata.
            OutputFile.Write(encodedChunk.EndState);
            OutputFile.Write(encodedChunk.BitIndex);
            OutputFile.Write(encodedChunk.UncompressedLength);
            for (int i = 0; i < encodedChunk.Data.Count; i++)
            {
                OutputFile.Write(encodedChunk.Data[i]);
            }
            InputFile.Close();
            OutputFile.Close();
        }
    }
}
