using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pikachu.Scripts.Common.Interfaces
{
    public interface IGridCell
    {
        public int ID { get; }
        public Vector3Int Position { get; set; }
        public ICreatureCell CreatureCell { get; set; }
        public void Clear();
    }
}
