using System;
using System.Collections.Generic;
using UnityEngine;

namespace GridSystem
{
    public class NumberUtil : MonoBehaviour
    {
        public static bool ContainsBytes(int value, int comparingValue) => (comparingValue ^ value) == (comparingValue - value);
        public static int Invert(int value, int maxValue) => Mathf.Abs(value - maxValue);
        public static float Invert(float value, float maxValue) => Mathf.Abs(value - maxValue);
        public static bool ContainsAnyBits(int value, int comparingValue)
        {
            int[] bits = SeparateBits(value);
            for (int i = 0; i < bits.Length; i++)
                if (ContainsBytes(bits[i], comparingValue))
                    return true;
            return false;
        }
        public static int[] SeparateBits(int value)
        {
            List<int> bits = new List<int>();
            int comparingValue = 1;
            for (int i = 0; i < 32; i++)
            {
                if ((comparingValue & value) != 0)
                    bits.Add(comparingValue);
                comparingValue <<= 1;
            }
            return bits.ToArray();
        }
    }
}