using UnityEngine;
using Pikachu.Scripts.Common.DataStructs;
using Pikachu.Scripts.Common.Enumerations;

namespace Pikachu.Scripts.Common.Interfaces
{
    public interface ICreatureCell
    {
        public int ID { get; }
        public CreatureData Data { get; }
        public void SetCellData(CreatureData data);
        public void SetChooseBorderActive(CreatureCellState state);
        public void ResetCell();
        public void Clear();
    }
}
