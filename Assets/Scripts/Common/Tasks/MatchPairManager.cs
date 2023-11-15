using System;
using Pikachu.Scripts.Common.Enumerations;
using Pikachu.Scripts.Common.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pikachu.Scripts.Common.Tasks
{
    public class MatchPairManager
    {
        private readonly GridCellManager _gridCellManager;
        private readonly CheckPairTask _checkPairTask;
        private readonly Action _onGameComplete;

        private int _maxPairCount = 72;

        private IGridCell _firstSlot = null;
        private IGridCell _secondSlot = null;


        public MatchPairManager(GridCellManager gridCellManager, CheckPairTask checkPairTask, Action onGameComplete)
        {
            _gridCellManager = gridCellManager;
            _checkPairTask = checkPairTask;
            _onGameComplete = onGameComplete;
        }

        public void TakePosition(Vector3Int position)
        {
            if(_firstSlot == null)
            {
                var slot = _gridCellManager.Get(position);

                if (slot == null)
                    return;

                _firstSlot = slot;
                _firstSlot.CreatureCell.SetChooseBorderActive(CreatureCellState.Choose);
            }

            else
            {
                if (position == _firstSlot.Position)
                {
                    _firstSlot.CreatureCell.SetChooseBorderActive(CreatureCellState.Normal);
                    _firstSlot = null;
                }

                else
                {
                    if (_secondSlot == null)
                    {
                        var slot = _gridCellManager.Get(position);

                        if (slot == null)
                            return;

                        _secondSlot = slot;
                        _secondSlot.CreatureCell.SetChooseBorderActive(CreatureCellState.Choose);

                        ProcessPairMatch(_firstSlot, _secondSlot);
                    }
                }
            }

        }

        private void ProcessPairMatch(IGridCell firstSlot, IGridCell secondSlot)
        {
            bool isPairMatchable = _checkPairTask.CheckPairMatch(firstSlot.Position, secondSlot.Position);

            if (isPairMatchable)
            {
                _maxPairCount = _maxPairCount - 1;

                _firstSlot.Clear();
                _secondSlot.Clear();

                _gridCellManager.Clear(firstSlot.Position);
                _gridCellManager.Clear(secondSlot.Position);
            }

            else
            {
                _firstSlot.CreatureCell.SetChooseBorderActive(CreatureCellState.Normal);
                _secondSlot.CreatureCell.SetChooseBorderActive(CreatureCellState.Normal);
            }

            _firstSlot = null;
            _secondSlot = null;

            if (_maxPairCount <= 0)
            {
                _maxPairCount = 72;
                _onGameComplete?.Invoke();
            }
        }
    }
}
