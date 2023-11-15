using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pikachu.Scripts.Common.Tasks;
using Pikachu.Scripts.Common.Factories;
using Pikachu.Scripts.Common.Database;
using Pikachu.Scripts.Common.DataStructs;

namespace Pikachu.Scripts.Common.GameBoard
{
    public class PikachuGameBoard : MonoBehaviour
    {
        [SerializeField] private Vector2Int boardSize;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Transform gridSlotContainer;
        [SerializeField] private CreatureDatabase creatureDatabase;
        [SerializeField] private CreatureCell creatureCell;
        [SerializeField] private GridSlotCell gridSlotCell;
        [SerializeField] private BoardInput boardInput;

        private DrawPairLineTask _drawPairLine;
        private CheckPairTask _checkPairTask;
        private GridCellManager _gridCellManager;
        private CreatureFactory _creatureFactory;
        private MatchPairManager _matchPairManager;
        private BoardFillTask _boardFillTask;
        private SuggestTask _suggestTask;
        private ProcessInputTask _processInputTask;

        private List<GridSlotCell> _gridSlotCells = new List<GridSlotCell>();

        private void Awake()
        {
            Initialize();
            GenerateGridSlot();
            GenerateBoard();
        }

        private void Initialize()
        {
            _gridCellManager = new GridCellManager();
            _creatureFactory = new CreatureFactory(creatureCell);
            _drawPairLine = new DrawPairLineTask();
            _checkPairTask = new CheckPairTask(_gridCellManager, _drawPairLine);
            _matchPairManager = new MatchPairManager(_gridCellManager, _checkPairTask, GenerateBoard);
            _boardFillTask = new BoardFillTask(creatureDatabase.Size, 24, boardSize);
            _suggestTask = new SuggestTask(_gridCellManager, _checkPairTask);
            _processInputTask = new ProcessInputTask(_matchPairManager, boardInput);
        }

        private void GenerateGridSlot()
        {
            for (int i = 0; i < boardSize.x; i++)
            {
                for (int j = 0; j < boardSize.y; j++)
                {
                    GridSlotCell slot = SimplePool.Spawn(gridSlotCell, gridSlotContainer
                                                         , new Vector3(i, j), Quaternion.identity);

                    Vector3Int position = new Vector3Int(i, j);
                    slot.GridPosition = position;
                    slot.gameObject.name = $"Grid Slot Cell: {i},{j}";
                    _gridSlotCells.Add(slot);
                }
            }
        }

        private void GenerateBoard()
        {
            _boardFillTask.Fill();
            _gridCellManager.ClearAll();

            for (int i = 0; i < _gridSlotCells.Count; i++)
            {
                Vector3Int position = _gridSlotCells[i].GridPosition;

                GridCell gridCell;
                int rand = _boardFillTask.CreatureIDs[position.x * boardSize.y + position.y];
                CreatureData cellData = creatureDatabase.CreatureImages[rand];
                CreatureCell cell = _creatureFactory.Create(cellData);

                cell.transform.position = position;
                cell.ResetCell();
                gridCell = new GridCell(position, cell);

                _gridCellManager.Add(position, gridCell);
            }

            mainCamera.transform.position = new Vector3((boardSize.x - 1) / 2.0f
                                                        , (boardSize.y - 1) / 2.0f + 0.5f
                                                        , -10f);
        }

        public void Shuffle()
        {
            _gridCellManager.Shuffle();
        }

        public void Suggest()
        {
            int iterateCount = 0;

            while(iterateCount < 100)
            {
                iterateCount++;
                if (_suggestTask.Suggest())
                    break;
            }
        }

        private void OnDestroy()
        {
            _checkPairTask.Dispose();
            _boardFillTask.Dispose();
            _gridCellManager.Dispose();
        }
    }
}
