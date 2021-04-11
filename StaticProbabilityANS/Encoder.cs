using AsymmetricNumeralSystemsTestPlatform.BitStreamIO;
using AsymmetricNumeralSystemsTestPlatform.SymbolStreamIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.StaticProbabilityANS
{
    class Encoder
    {
        public readonly Config Config;

        /// <summary>
        ///  Initialize Static Probability Asymmetric Numeral Systems  Encoder.
        /// </summary>
        /// <param name="alphabet">Symbol alphabet.</param>
        /// <param name="frequencyCounts">Frequency counts of the symbol alphabet. Frequency
        /// count is selected using same index as symbol alphabet. Frequency counts must sum 
        /// to a number that is power of two. </param>
        public Encoder(int[] alphabet, int[] frequencyCounts)
        {
            Config = new Config(alphabet, frequencyCounts);
        }

        /// <summary>
        ///  Encodes a symbol into the working state.
        /// </summary>
        /// <param name="symbol">Symbol to be encoded.</param>
        /// <param name="state">Working state.</param>
        public void EncodeSymbol(int symbol, ref int state)
        {
            state = ((int)((float)state / Config.FrequencyCounts[symbol])) *
                    Config.TotalFrequencyCounts +
                    Config.CumulativeFrequencyCounts[symbol] +
                    (state % Config.FrequencyCounts[symbol]);
        }

        /// <summary>
        ///  Renormalizes the working state by outputting the LSB into
        ///  the ANS bitstream.
        /// </summary>
        /// <param name="symbol">Current working symbol.</param>
        /// <param name="output">Output ANS bitstream.</param>
        /// <param name="state">Working state.</param>
        public void EncodeRenormalizeState(int symbol, BitStreamIO.Writer output, ref int state)
        {
            while (state >= (Config.FrequencyCounts[symbol] << 1))
            {
                output.PutBit((state & 1));
                state >>= 1;
            }
        }

        /// <summary>
        ///  Encodes a list of bytes into metadata and bitstream.
        /// </summary>
        /// <param name="input">Input byte list</param>
        public BitStream EncodeANSBitStream(List<byte> input)
        {
            var Input = new SymbolStreamIO.Reader(input);
            var Output = new BitStreamIO.Writer();
            int state = Config.TotalFrequencyCounts;
            int symbol;
            while (Input.HasData())
            {
                symbol = Input.GetSymbol();
                EncodeRenormalizeState(symbol, Output, ref state);
                EncodeSymbol(symbol, ref state);
            }
            Output.Terminate(input.Count, state);
            return Output.BitStream;
        }
    }
}
