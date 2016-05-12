using System.Collections.Generic;

namespace FourInARow
{
    static class PatternSearchExtension
    {
        static readonly int[] Empty = new int[0];

        public static int[] Locate(this int[] self, int[] candidate)
        {
            if (IsEmptyLocate(self, candidate))
                return Empty;

            List<int> list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? Empty : list.ToArray();
        }

        static bool IsMatch(int[] array, int position, int[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
            {
                if (array[position + i] != candidate[i])
                    return false;
            }

            return true;
        }

        static bool IsEmptyLocate(int[] array, int[] candidate)
        {
            return array == null ||
                candidate == null ||
                array.Length == 0 ||
                candidate.Length == 0 ||
                candidate.Length > array.Length;
        }
    }
}
