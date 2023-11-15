using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Pikachu.Scripts.Common.Tasks
{
    public class BoardFillTask : IDisposable
    {
        private readonly int _maxRange;
        private readonly int _setCount;
        private readonly Vector2Int _boardSize;

        private HashSet<int> _idSet = new HashSet<int>();
        private List<int> _creatureIDs = new List<int>();

        public List<int> CreatureIDs => _creatureIDs;
        public HashSet<int> IDSet => _idSet;

        public BoardFillTask(int maxRange, int setCount, Vector2Int boardSize)
        {
            _maxRange = maxRange;
            _setCount = setCount;
            _boardSize = boardSize;
        }

        public void Fill()
        {
            TakeIDSet();
            _creatureIDs.Clear();

            int totalCount = _boardSize.x * _boardSize.y;

            while (_creatureIDs.Count < totalCount)
            {
                _creatureIDs.AddRange(_idSet);
            }

            Shuffle(_creatureIDs);
        }

        private void TakeIDSet()
        {
            int rand;
            _idSet.Clear();

            while(_idSet.Count < _setCount)
            {
                rand = Random.Range(0, _maxRange);
                _idSet.Add(rand);
            }
        }

        private void Shuffle(List<int> numbers)
        {
            int rand, temp;
            int count = numbers.Count;

            for (int i = 0; i < count; i++)
            {
                rand = Random.Range(0, count - 1);
                temp = numbers[i];
                numbers[i] = numbers[rand];
                numbers[rand] = temp;
            }
        }

        public void Dispose()
        {
            _idSet.Clear();
            _creatureIDs.Clear();
        }
    }
}
