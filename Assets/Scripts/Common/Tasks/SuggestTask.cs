using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pikachu.Scripts.Common.Interfaces;
using Pikachu.Scripts.Common.Enumerations;

namespace Pikachu.Scripts.Common.Tasks
{
    public class SuggestTask
    {
        private readonly GridCellManager _gridCellManager;
        private readonly CheckPairTask _checkPairTask;

        private bool _hasAPairMatch = false;
        private List<Vector3Int> _suggestedCells;

        public SuggestTask(GridCellManager gridCellManager, CheckPairTask checkPairTask)
        {
            _gridCellManager = gridCellManager;
            _checkPairTask = checkPairTask;
        }

        public bool Suggest()
        {
            _hasAPairMatch = false;
            Vector3Int pointA, pointB;

            IGridCell cellA, cellB;
            IGridCell randomCell = _gridCellManager.GetRandomCell();
            
            if (randomCell == null)
                return false;

            _suggestedCells = _gridCellManager.GetCellsWithValue(randomCell.ID);

            for (int i = 0; i < _suggestedCells.Count; i++)
            {
                pointA = _suggestedCells[i];

                for (int j = 0; j < _suggestedCells.Count; j++)
                {
                    if (i == j)
                        continue;

                    pointB = _suggestedCells[j];
                    _hasAPairMatch = _checkPairTask.CheckPairMatch(pointA, pointB);

                    if (_hasAPairMatch)
                    {
                        cellA = _gridCellManager.Get(pointA);
                        cellB = _gridCellManager.Get(pointB);
                        
                        cellA.CreatureCell.SetChooseBorderActive(CreatureCellState.Suggest);
                        cellB.CreatureCell.SetChooseBorderActive(CreatureCellState.Suggest);

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
