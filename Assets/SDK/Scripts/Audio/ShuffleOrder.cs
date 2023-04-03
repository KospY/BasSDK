namespace ThunderRoad
{
    using UnityEngine;

    /// <summary>
    /// Class use to shufle order and then go threw every index before reshuffle
    /// Reshuffle will not use the last order first and last shhuffle index to avoid repetition 
    /// </summary>
    public class ShuffleOrder
    {
        private int _count;
        private int _index = 0;
        private int[] _shuffleOrder = null;

        /// <summary>
        /// Create an order of shuffle index array from 0 to count (count not include)
        /// </summary>
        /// <param name="count">The size of shuffle array. If count = 2 ping pong effect. count must be > 0 </param>
        public ShuffleOrder(int count)
        {
            _count = count;
            _index = 0;
            _shuffleOrder = null;

            if(_count > 2)
            {
                Shuffle();
            }
        }

        /// <summary>
        /// Create an order of shuffle index array from 0 to count (count not include)
        /// </summary>
        /// <param name="count">The size of shuffle array. If count = 2 ping pong effect. count must be > 0</param>
        /// <param name="notFirstIndex">Index that should not be in first in the shuffle order</param>
        public ShuffleOrder(int count, int notFirstIndex)
        {
            _count = count;
            _index = 0;

            if (_count > 2)
            {
                _shuffleOrder = new int[_count];
                _shuffleOrder[0] = notFirstIndex;

                for (int i = 1; i < _count; i++)
                {
                    if (i <= _shuffleOrder[0])
                    {
                        _shuffleOrder[i] = i - 1;
                    }
                    else
                    {
                        _shuffleOrder[i] = i;
                    }
                }

                Shuffle();
                return;
            }

            // start ping pong from other index than notFirstIndex 
            _index = notFirstIndex;
            Next();
        }

        /// <summary>
        /// Return current shuffle index and change index order for the next time
        /// </summary>
        /// <returns>Return the current shuffle index</returns>
        public int Next()
        {
            if (_count <= 0)
            {
                return -1;
            }

            if (_count <= 2)
            {
                int index = _index;
                _index++;
                if (_index >= _count)
                {
                    _index = 0;
                }

                return index;
            }

            // If all order has been used : reshufle
            if (_index >= _count)
            {
                _index = 0;
                Shuffle();
            }

            int shuffleIndex = _shuffleOrder[_index];

            _index++;

            return shuffleIndex;
        }

        private void Shuffle()
        {
            if (_shuffleOrder == null)
            {
                int[] defaultOrder = new int[_count];
                _shuffleOrder = new int[_count];
                for (int i = 0; i < _count; i++)
                {
                    defaultOrder[i] = i;
                }

                for (int i = 0; i < _count; i++)
                {
                    int max = _count - i;
                    int randomIndex = Random.Range(0, max);
                    _shuffleOrder[i] = defaultOrder[randomIndex];
                    defaultOrder[randomIndex] = defaultOrder[max - 1];
                }
            }
            else
            {
                int[] previousOrder = _shuffleOrder;
                _shuffleOrder = new int[_count];

                int randomIndex = Random.Range(1, _count - 1); // First element need to be different from first and last element of the previous order
                _shuffleOrder[0] = previousOrder[randomIndex];
                previousOrder[randomIndex] = previousOrder[_count - 1];

                for (int i = 1; i < _count; i++)
                {
                    int max = _count - i;
                    randomIndex = Random.Range(0, max);
                    _shuffleOrder[i] = previousOrder[randomIndex];
                    previousOrder[randomIndex] = previousOrder[max - 1];
                }
            }
        }
    }
}