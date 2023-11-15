using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pikachu.Scripts.Common.Interfaces;
using Pikachu.Scripts.Common.DataStructs;
using Pikachu.Scripts.Common.GameBoard;

namespace Pikachu.Scripts.Common.Factories
{
    public class CreatureFactory : IFactory<CreatureData, CreatureCell>
    {
        private readonly CreatureCell _creatureCell;

        public CreatureFactory(CreatureCell creatureCell)
        {
            _creatureCell = creatureCell;
        }

        public CreatureCell Create(CreatureData param)
        {
            CreatureCell cell = SimplePool.Spawn(_creatureCell);
            cell.transform.SetParent(CreatureCellContainer.InstanceTransform);
            cell.SetCellData(param);
            return cell;
        }
    }
}
