using UnityEngine;

namespace Marty
{
    public class CombinationLock<T> where T : System.Enum
    {
        private T[] solution;
        private T[] slots;

        public CombinationLock(T[] solution)
        {
            this.solution = (T[])solution.Clone();
            slots = new T[solution.Length];
        }

        public void Shuffle()
        {
            System.Array values = System.Enum.GetValues(typeof(T));
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i] = (T)values.GetValue(Random.Range(0, values.Length));
            }
        }
    }
}
