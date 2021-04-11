using AsymmetricNumeralSystemsTestPlatform.BitStreamIO;
using AsymmetricNumeralSystemsTestPlatform.SymbolStreamIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsymmetricNumeralSystemsTestPlatform.StaticProbabilityANS
{
    struct Config
    {
        public readonly int[] Alphabet;
        public readonly int[] FrequencyCounts;
        public readonly int TotalFrequencyCounts;
        public readonly float[] Probabilities;
        public readonly int[] CumulativeFrequencyCounts;

        /// <summary>
        ///  Checks if number is power of two
        /// </summary>
        /// <param name="x">Input number.</param>
        private static bool IsPowerOfTwo(int x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }

        /// <summary>
        ///  Initialize and sanity check config.
        /// </summary>
        /// <param name="alphabet">Symbol alphabet.</param>
        /// <param name="frequencyCounts">Frequency counts of the symbol alphabet. Frequency
        /// count is selected using same index as symbol alphabet. Frequency counts must sum 
        /// to a number that is power of two. </param>
        public Config(int[] alphabet, int[] frequencyCounts)
        {

            Alphabet = alphabet;
            FrequencyCounts = frequencyCounts;
            if (!(Alphabet.Length >= 2) || !IsPowerOfTwo(Alphabet.Length))
            {
                throw new ApplicationException("Alphabet must be 2 or more symbols, and it must be number that is power of two.");
            }
            if (Alphabet.Length != FrequencyCounts.Length)
            {
                throw new ApplicationException("Alphabet and Frequency count length missmatch");
            }

            int Length = Alphabet.Length;
            TotalFrequencyCounts = 0;
            // Calculate total frequency counts (samples).
            for (int i = 0; i < Length; i++)
            {
                TotalFrequencyCounts += FrequencyCounts[i];
            }
            if (!IsPowerOfTwo(TotalFrequencyCounts))
            {
                throw new ApplicationException("Frequency counts must sum to a number that is power of two.");
            }
            int Temp = 0;
            Probabilities = new float[Length];
            CumulativeFrequencyCounts = new int[Length];
            for (int i = 0; i < Length; i++)
            {
                // Calculates the probabilities of each symbol in the alphabet,
                // based on the frequency vs total samples (total_frequency counts).
                Probabilities[i] = (float)FrequencyCounts[i] / (float)TotalFrequencyCounts;
                // Calculates the cumulative frequency counts
                // (total counts "before" current alphabet symbol).
                CumulativeFrequencyCounts[i] = Temp;
                Temp += FrequencyCounts[i];
            }
        }
    }
}
