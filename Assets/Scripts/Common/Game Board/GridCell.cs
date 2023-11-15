using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pikachu.Scripts.Common.Interfaces;

namespace Pikachu.Scripts.Common.GameBoard
{
    public class GridCell : IGridCell
    {
        public int ID => CreatureCell.ID;
        public Vector3Int Position { get; set; }
        public ICreatureCell CreatureCell { get; set; }

        public GridCell(Vector3Int position, ICreatureCell creatureCell)
        {
            Position = position;
            CreatureCell = creatureCell;
        }

        public void Clear()
        {
            if (CreatureCell != null)
                CreatureCell.Clear();
        }
    }
}
