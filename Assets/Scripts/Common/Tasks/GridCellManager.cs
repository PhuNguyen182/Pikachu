using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pikachu.Scripts.Common.Interfaces;
using Pikachu.Scripts.Common.DataStructs;
using Random = UnityEngine.Random;

namespace Pikachu.Scripts.Common.Tasks
{
    public class GridCellManager : IDisposable
    {
        private Dictionary<Vector3Int, IGridCell> _kvp;

        public IGridCell GetRandomCell()
        {
            if (_kvp.Count == 0)
                return null;

            int randomIndex = Random.Range(0, _kvp.Count);
            Vector3Int key = _kvp.Keys.ElementAt(randomIndex);
            return _kvp[key];
        }

        public GridCellManager()
        {
            _kvp = new Dictionary<Vector3Int, IGridCell>();
        }

        public void Add(Vector3Int position, IGridCell gridCell)
        {
            _kvp.Add(position, gridCell);
        }

        public IGridCell Get(Vector3Int position)
        {
            if (_kvp.TryGetValue(position, out var cell))
                return cell;

            return null;
        }

        public void Shuffle()
        {
            int rand;
            int totalCount = _kvp.Count;

            Vector3Int firstKey, secondKey;
            IGridCell firstCell, secondCell;
            CreatureData firstData, secondData;

            List<Vector3Int> keys = _kvp.Keys.ToList();

            for (int i = 0; i < totalCount; i++)
            {
                rand = Random.Range(0, totalCount);

                if (rand == i)
                    rand = (rand + 1) % totalCount;

                firstKey = keys[i];
                secondKey = keys[rand];

                firstCell = _kvp[firstKey];
                secondCell = _kvp[secondKey];

                if (firstCell.ID == secondCell.ID)
                    continue;

                firstData = new CreatureData 
                { 
                    ID = firstCell.CreatureCell.Data.ID,
                    Creature = firstCell.CreatureCell.Data.Creature
                };

                secondData = new CreatureData 
                { 
                    ID = secondCell.CreatureCell.Data.ID,
                    Creature = secondCell.CreatureCell.Data.Creature
                };

                firstCell.CreatureCell.SetCellData(secondData);
                secondCell.CreatureCell.SetCellData(firstData);
            }
        }

        public List<Vector3Int> GetCellsWithValue(int value)
        {
            List<Vector3Int> cells = new List<Vector3Int>();

            foreach (KeyValuePair<Vector3Int, IGridCell> item in _kvp)
            {
                if (item.Value.CreatureCell.ID == value)
                    cells.Add(item.Key);
            }

            return cells;
        }

        public void Clear(Vector3Int position)
        {
            if (_kvp.ContainsKey(position))
                _kvp.Remove(position);
        }

        public void ClearAll()
        {
            _kvp.Clear();
        }

        public void Dispose()
        {
            _kvp.Clear();
        }
    }
}
