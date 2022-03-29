using UnityEngine;

namespace Marty
{
    public class ShuffledIndex
    {
        private int[] indexMap;
        private int currentPos;

        public ShuffledIndex(int size)
        {
            // Initialize index map
            indexMap = new int[size];
            for (int i = 0; i < size; i++)
            {
                indexMap[i] = i;
            }

            // Initialize position counter
            currentPos = -2;
        }

        public static implicit operator int(ShuffledIndex index)
        {
            return (index.currentPos < 0) ? index.GetNextValue() : index.indexMap[index.currentPos];
        }

        public int GetNextValue()
        {
            // Return -1 if index size is zero
            if (indexMap.Length == 0)
            {
                return -1;
            }

            // Move to next item
            currentPos++;

            // Do we need to shuffle map?
            if ((currentPos < 0) || (currentPos >= indexMap.Length))
            {
                // Save last used index
                int lastIndex = (currentPos < 0) ? -1 : indexMap[currentPos - 1];

                // Shuffle map
                for (int i = indexMap.Length - 1; i > 0; i--)
                {
                    int j = Random.Range(0, i);
                    int tmp = indexMap[i];
                    indexMap[i] = indexMap[j];
                    indexMap[j] = tmp;
                }

                // Make sure we do not reuse last index immediately
                if ((indexMap.Length > 1) && (lastIndex == indexMap[0]))
                {
                    indexMap[0] = indexMap[1];
                    indexMap[1] = lastIndex;
                }

                // Move to first item
                currentPos = 0;
            }

            // Return selected index
            return indexMap[currentPos];
        }
    }
}
