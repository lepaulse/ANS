using AsymmetricNumeralSystemsTestPlatform.BitStreamIO;
using AsymmetricNumeralSystemsTestPlatform.SymbolStreamIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.StaticProbabilityANS
{
    class Decoder
    {
        public readonly Config Config;

        /// <summary>
        ///  Initialize Static Probability Asymmetric Numeral Systems Decoder.
        /// </summary>
        /// <param name="alphabet">Symbol alphabet.</param>
        /// <param name="frequencyCounts">Frequency counts of the symbol alphabet. Frequency
        /// count is selected using same index as symbol alphabet. Frequency counts must sum 
        /// to a number that is power of two. </param>
        public Decoder(int[] alphabet, int[] frequencyCounts)
        {
            Config = new Config(alphabet, frequencyCounts);
        }

        /// <summary>
        ///  Helper function for cumulative frequency count invertion.
        ///  TODO: Possible to make static array?
        /// </summary>
        /// <param name="slot">symbol slot selector</param>
        private int Inverse_cumulative_frequency_count(int slot)
        {
            for (int i = 0; i < Config.FrequencyCounts.Length; i++)
            {
                if (slot < Config.CumulativeFrequencyCounts[i])
                {
                    return i - 1;
                }
            }
            return Config.FrequencyCounts.Length - 1;
        }

        /// <summary>
        ///  Decodes a symbol from the working state. Always returns
        ///  a valid decoded symbol value.
        /// </summary>
        /// <param name="state">Working state</param>
        public int DecodeSymbol(ref int state)
        {
            int slot = state % Config.TotalFrequencyCounts;
            int symbol = Inverse_cumulative_frequency_count(slot);
            state = ((int)Math.Floor(((float)state / (float)Config.TotalFrequencyCounts))) *
                Config.FrequencyCounts[symbol] +
                slot -
                Config.CumulativeFrequencyCounts[symbol];
            return symbol;
        }

        /// <summary>
        ///  Renormalizes the working state by appending a bit from
        ///  the ANS bitstream. If there still excists bits in the 
        ///  ANS bitstream.
        /// </summary>
        /// <param name="input">Input ANS bitstream.</param>
        /// <param name="state">Working state.</param>
        private void RenormalizeDecode(BitStreamIO.Reader input, ref int state)
        {
            while (state < Config.TotalFrequencyCounts)
            {
                state <<= 1;
                if (input.HasBits())
                {
                    state ^= input.GetBit();
                }
            }
        }

        /// <summary>
        ///  Decodes metadata and bitstream into a list of bytes.
        /// </summary>
        /// <param name="input">Input ANS bitstream and metadata</param>
        public List<byte> DecodeANSBitStream(BitStreamIO.Reader input)
        {
            SymbolStreamIO.Writer output = new SymbolStreamIO.Writer();
            int state = input.BitStream.EndState;
            for (int i = 0; i < (input.BitStream.UncompressedLength * 8); i++)
            {
                output.PutSymbol(DecodeSymbol(ref state));
                RenormalizeDecode(input, ref state);
            }
            if (state != Config.TotalFrequencyCounts)
            {
                throw new ApplicationException("Decoding data corruption. Expected state " +
                    Config.TotalFrequencyCounts.ToString() + ", Received " + state.ToString() + ".");
            }
            output.Reverse();
            return output.SymbolStream.Data;
        }
    }
}
