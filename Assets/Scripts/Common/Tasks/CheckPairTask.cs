using System;
using Pikachu.Scripts.Common.Enumerations;
using Pikachu.Scripts.Common.Interfaces;
using UnityEngine;

namespace Pikachu.Scripts.Common.Tasks
{
    public class CheckPairTask : IDisposable
    {
        private readonly GridCellManager _gridCellManager;
        private readonly DrawPairLineTask _drawPairLineTask;

        private Vector3[] positions;

        private const int X_MIN = -1;
        private const int Y_MIN = -1;
        private const int X_MAX = 16;
        private const int Y_MAX = 9;

        public CheckPairTask(GridCellManager gridCellManager, DrawPairLineTask drawPairLineTask)
        {
            _gridCellManager = gridCellManager;
            _drawPairLineTask = drawPairLineTask;
        }

        public bool CheckPairMatch(Vector3Int firstPoint, Vector3Int secondPoint)
        {
            bool isPair;
            IGridCell firstSlot = _gridCellManager.Get(firstPoint);
            IGridCell secondSlot = _gridCellManager.Get(secondPoint);

            // If 2 cells are not the same, return false
            if (firstSlot.CreatureCell.ID != secondSlot.CreatureCell.ID)
                return false;

            if (firstPoint.x == secondPoint.x)
            {
                isPair = CheckLinePass(firstPoint, secondPoint, CheckLineDirection.Vertical);

                if (isPair) _drawPairLineTask.DrawLine(firstPoint, secondPoint);
                else isPair = CheckMoreLineY(firstPoint, secondPoint);

                return isPair;
            }

            else if (firstPoint.y == secondPoint.y)
            {
                isPair = CheckLinePass(firstPoint, secondPoint, CheckLineDirection.Horizontal);

                if (isPair) _drawPairLineTask.DrawLine(firstPoint, secondPoint);
                else isPair = CheckMoreLineX(firstPoint, secondPoint);

                return isPair;
            }

            else
            {
                isPair = CheckRectX(firstPoint, secondPoint);
                if (!isPair)
                    isPair = CheckRectY(firstPoint, secondPoint);
                if (!isPair)
                    isPair = CheckMoreLineX(firstPoint, secondPoint);
                if (!isPair)
                    isPair = CheckMoreLineY(firstPoint, secondPoint);

                return isPair;
            }
        }

        private bool CheckLineEmpty(Vector3Int pointA, Vector3Int pointB, CheckLineDirection lineDirection, bool ignoreFirstCell = false)
        {
            int length = 0;
            Vector3Int startPoint = default, endPoint, step = default;

            if (lineDirection == CheckLineDirection.Horizontal)
            {
                step = Vector3Int.right;
                startPoint = pointA.x < pointB.x ? pointA : pointB;
                endPoint = pointA.x > pointB.x ? pointA : pointB;
                length = endPoint.x - startPoint.x;
            }

            else if(lineDirection == CheckLineDirection.Vertical)
            {
                step = Vector3Int.up;
                startPoint = pointA.y < pointB.y ? pointA : pointB;
                endPoint = pointA.y > pointB.y ? pointA : pointB;
                length = endPoint.y - startPoint.y;
            }

            if (length == 0)
                return _gridCellManager.Get(startPoint) == null;

            for (int i = 0; i <= length; i++)
            {
                if (i == 0 && ignoreFirstCell)
                    continue;

                Vector3Int checkPosition = startPoint + step * i;
                IGridCell checkCell = _gridCellManager.Get(checkPosition);

                if (checkCell != null)
                    return false;
            }

            return true;
        }

        private bool CheckLinePass(Vector3Int firstPoint, Vector3Int secondPoint, CheckLineDirection lineDirection)
        {
            int length = 0;
            Vector3Int step = default;
            Vector3Int starterPoint = default;

            if (lineDirection == CheckLineDirection.Horizontal)
            {
                starterPoint = firstPoint.x < secondPoint.x ? firstPoint : secondPoint;
                length = Mathf.Abs(secondPoint.x - firstPoint.x);
                step = Vector3Int.right;
            }

            else if(lineDirection == CheckLineDirection.Vertical)
            {
                starterPoint = firstPoint.y < secondPoint.y ? firstPoint : secondPoint;
                length = Mathf.Abs(secondPoint.y - firstPoint.y);
                step = Vector3Int.up;
            }

            for (int i = 1; i <= length; i++)
            {
                Vector3Int nextPos = starterPoint + step * i;
                IGridCell nextSlot = _gridCellManager.Get(nextPos);

                if (nextSlot != null && i < length)
                    return false;
            }

            return true;
        }

        private bool CheckLineXEmptyAndCompare(Vector3Int pointA, Vector3Int pointB, int value, bool ignoreFirstCell = false)
        {
            IGridCell checkCell;
            Vector3Int checkPoint;
            Vector3Int startPoint = pointA, endPoint = pointB;
            Vector3Int step = startPoint.x < endPoint.x ? Vector3Int.right : Vector3Int.left;
            int length = Mathf.Abs(startPoint.x - endPoint.x);

            for (int i = 0; i < length; i++)
            {
                checkPoint = startPoint + step * i;
                checkCell = _gridCellManager.Get(checkPoint);

                if (i == 0 && ignoreFirstCell)
                    continue;
                
                else if (checkCell != null)
                    return false;
            }

            checkCell = _gridCellManager.Get(startPoint + step * length);
            return checkCell == null ? false : checkCell.CreatureCell.ID == value;
        }

        private bool CheckLineYEmptyAndCompare(Vector3Int pointA, Vector3Int pointB, int value, bool ignoreFirstCell = false)
        {
            IGridCell checkCell;
            Vector3Int checkPoint;
            Vector3Int startPoint = pointA, endPoint = pointB;
            Vector3Int step = startPoint.y < endPoint.y ? Vector3Int.up : Vector3Int.down;
            int length = Mathf.Abs(startPoint.y - endPoint.y);

            for (int i = 0; i < length; i++)
            {
                checkPoint = startPoint + step * i;
                checkCell = _gridCellManager.Get(checkPoint);
                
                if (i == 0 && ignoreFirstCell)
                    continue;

                else if (checkCell != null)
                    return false;
            }

            checkCell = _gridCellManager.Get(startPoint + step * length);
            return checkCell == null ? false : checkCell.CreatureCell.ID == value;
        }

        private bool CheckMoreLineX(Vector3Int firstPoint, Vector3Int secondPoint)
        {
            // In case of invalid pair in both row or rect, check this U shape
            bool isPaired = false;
            Vector3Int castedPoint = default;
            Vector3Int pointA = firstPoint;
            Vector3Int pointB = secondPoint;
            Vector3Int step2CheckPoint;
            IGridCell firstSlot = _gridCellManager.Get(pointA);

            // Upper check
            while (pointA.y <= Y_MAX)
            {
                pointA += Vector3Int.up;
                castedPoint = new Vector3Int(pointB.x, pointA.y);

                if (_gridCellManager.Get(pointA) != null)
                {
                    isPaired = false;
                    break;
                }

                if (CheckLineEmpty(pointA, castedPoint, CheckLineDirection.Horizontal) && pointA.y >= pointB.y)
                {
                    step2CheckPoint = castedPoint;
                    isPaired = CheckLineYEmptyAndCompare(step2CheckPoint, pointB, firstSlot.CreatureCell.ID);
                    break;
                }
            }

            // Downer check
            if (!isPaired)
            {
                // Reset check position
                pointA = firstPoint;
                while (pointA.y >= Y_MIN)
                {
                    pointA += Vector3Int.down;
                    castedPoint = new Vector3Int(pointB.x, pointA.y);

                    if (_gridCellManager.Get(pointA) != null)
                    {
                        isPaired = false;
                        break;
                    }

                    if (CheckLineEmpty(pointA, castedPoint, CheckLineDirection.Horizontal) && pointA.y <= pointB.y)
                    {
                        step2CheckPoint = castedPoint;
                        isPaired = CheckLineYEmptyAndCompare(step2CheckPoint, pointB, firstSlot.CreatureCell.ID);
                        break;
                    }
                }
            }

            if (isPaired)
                _drawPairLineTask.DrawLine(firstPoint, pointA, castedPoint, secondPoint);

            return isPaired;
        }

        private bool CheckMoreLineY(Vector3Int firstPoint, Vector3Int secondPoint)
        {
            // In case of invalid pair in both row or rect, check this U shape
            bool isPaired = false;
            Vector3Int castedPoint = default;
            Vector3Int pointA = firstPoint;
            Vector3Int pointB = secondPoint;
            Vector3Int step2CheckPoint;
            IGridCell firstSlot = _gridCellManager.Get(pointA);

            // Upper check
            while (pointA.x < X_MAX)
            {
                pointA += Vector3Int.right;
                castedPoint = new Vector3Int(pointA.x, pointB.y);

                if (_gridCellManager.Get(pointA) != null)
                {
                    isPaired = false;
                    break;
                }

                if (CheckLineEmpty(pointA, castedPoint, CheckLineDirection.Vertical) && pointA.x >= pointB.x)
                {
                    step2CheckPoint = castedPoint;
                    isPaired = CheckLineXEmptyAndCompare(step2CheckPoint, pointB, firstSlot.CreatureCell.ID);
                    break;
                }
            }

            // Downer check
            if (!isPaired)
            {
                // Reset check position
                pointA = firstPoint;
                while (pointA.x > X_MIN)
                {
                    pointA += Vector3Int.left;
                    castedPoint = new Vector3Int(pointA.x, pointB.y);

                    if (_gridCellManager.Get(pointA) != null)
                    {
                        isPaired = false;
                        break;
                    }

                    if (CheckLineEmpty(pointA, castedPoint, CheckLineDirection.Vertical) && pointA.x <= pointB.x)
                    {
                        step2CheckPoint = castedPoint;
                        isPaired = CheckLineXEmptyAndCompare(step2CheckPoint, pointB, firstSlot.CreatureCell.ID);
                        break;
                    }
                }
            }

            if (isPaired)
                _drawPairLineTask.DrawLine(firstPoint, pointA, castedPoint, secondPoint);

            return isPaired;
        }

        private bool CheckRectX(Vector3Int firstPoint, Vector3Int secondPoint)
        {
            bool isPaired = false, isIgnore;
            Vector3Int checkPoint = default;
            Vector3Int castedPoint = default;
            Vector3Int startPoint = firstPoint.x < secondPoint.x ? firstPoint : secondPoint;
            Vector3Int endPoint = startPoint == secondPoint ? firstPoint : secondPoint;
            Vector3Int step = startPoint.x < endPoint.x ? Vector3Int.right : Vector3Int.left;

            IGridCell firstCell = _gridCellManager.Get(startPoint);
            int length = Mathf.Abs(startPoint.x - endPoint.x);

            for (int i = 0; i <= length; i++)
            {
                isIgnore = i == 0 || i == length;
                checkPoint = startPoint + step * i;
                castedPoint = new Vector3Int(checkPoint.x, endPoint.y);

                if (CheckLineEmpty(checkPoint, castedPoint, CheckLineDirection.Vertical, isIgnore))
                {
                    if (CheckLineEmpty(startPoint, castedPoint, CheckLineDirection.Horizontal, true))
                        isPaired = CheckLineXEmptyAndCompare(castedPoint, endPoint, firstCell.CreatureCell.ID, false);
                    break;
                }
            }

            if (isPaired)
            {
                if(startPoint == checkPoint) _drawPairLineTask.DrawLine(startPoint, castedPoint, endPoint);
                else _drawPairLineTask.DrawLine(startPoint, checkPoint, castedPoint, endPoint);
            }

            return isPaired;
        }

        private bool CheckRectY(Vector3Int firstPoint, Vector3Int secondPoint)
        {
            bool isPaired = false, isIgnore;
            Vector3Int checkPoint = default;
            Vector3Int castedPoint = default;
            Vector3Int startPoint = firstPoint.y < secondPoint.y ? firstPoint : secondPoint;
            Vector3Int endPoint = startPoint == secondPoint ? firstPoint : secondPoint;
            Vector3Int step = startPoint.y < endPoint.y ? Vector3Int.up : Vector3Int.down;

            IGridCell firstCell = _gridCellManager.Get(startPoint);
            int length = Mathf.Abs(startPoint.y - endPoint.y);

            for (int i = 0; i <= length; i++)
            {
                isIgnore = i == 0 || i == length;
                checkPoint = startPoint + step * i;
                castedPoint = new Vector3Int(endPoint.x, checkPoint.y);

                if (CheckLineEmpty(checkPoint, castedPoint, CheckLineDirection.Horizontal, isIgnore))
                {
                    if (CheckLineEmpty(startPoint, castedPoint, CheckLineDirection.Vertical, true))
                        isPaired = CheckLineYEmptyAndCompare(castedPoint, endPoint, firstCell.CreatureCell.ID, false);
                    break;
                }
            }

            if (isPaired)
            {
                if(startPoint == checkPoint) _drawPairLineTask.DrawLine(startPoint, castedPoint, endPoint);
                else _drawPairLineTask.DrawLine(startPoint, checkPoint, castedPoint, endPoint);
            }

            return isPaired;
        }

        public void Dispose()
        {
            if (positions != null)
                Array.Clear(positions, 0, positions.Length);
        }
    }
}
