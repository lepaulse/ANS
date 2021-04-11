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
    class Reader
    {
        public readonly int[] Alphabet = { 0, 1 };
        public readonly int[] FrequencyCounts = { 224, 32 };

        public void StreamDecodeFile(string inputFile, string outputFile)
        {
            StaticProbabilityANS.Decoder Decoder = new StaticProbabilityANS.Decoder(Alphabet, FrequencyCounts);
            BinaryReader InputFile = new BinaryReader(File.Open(inputFile, FileMode.Open));
            int endState = InputFile.ReadInt32();
            int EndBitOffset = InputFile.ReadInt32();
            long UncompressedLength = InputFile.ReadInt64();
            List<byte> input = new List<byte>();
            for (int i = 16; i < InputFile.BaseStream.Length; i++)
            {
                input.Add(InputFile.ReadByte());
            }
            BitStreamIO.Reader encodedChunk = new BitStreamIO.Reader(endState, EndBitOffset, input, UncompressedLength);
            List<byte> output = Decoder.DecodeANSBitStream(encodedChunk);
            BinaryWriter OutputFile = new BinaryWriter(File.Open(outputFile, FileMode.Create));
            for (int i = 0; i < output.Count; i++)
            {
                OutputFile.Write(output[i]);
            }
            InputFile.Close();
            OutputFile.Close();
        }
    }
}
