using Pikachu.Scripts.Common.GameBoard;

namespace Pikachu.Scripts.Common.Tasks
{
    public class ProcessInputTask
    {
        private readonly MatchPairManager _matchPairManager;

        private GridSlotCell _currentGridSlotCell;

        public ProcessInputTask(MatchPairManager matchPairManager, BoardInput boardInput)
        {
            _matchPairManager = matchPairManager;

            boardInput.OnPress = OnPress;
            boardInput.OnRelease = OnRelease;
        }

        private void OnPress(GridSlotCell gridSlotCell)
        {
            _currentGridSlotCell = gridSlotCell;
        }

        private void OnRelease(bool release)
        {
            if (_currentGridSlotCell == null)
                return;

            _matchPairManager.TakePosition(_currentGridSlotCell.GridPosition);
            _currentGridSlotCell = null;
        }
    }
}
