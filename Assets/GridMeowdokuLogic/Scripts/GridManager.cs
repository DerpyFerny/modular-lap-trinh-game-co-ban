using System.Collections.Generic;
using UnityEngine;

namespace GridDragDrop
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance { get; private set; }

        [Header("Grid Config")]
        [Tooltip("Số hàng của lưới, VD: 4 hoặc 9")]
        [SerializeField] private int rows = 4;

        [Tooltip("Số cột của lưới, VD: 4 hoặc 9")]
        [SerializeField] private int cols = 4;

        [Header("References")]
        [Tooltip("Panel/RectTransform có gắn GridLayoutGroup, chứa các cell")]
        [SerializeField] private RectTransform gridContainer;

        [Tooltip("Prefab cell (đã có GridCell.cs + Image nền)")]
        [SerializeField] private GridCell cellPrefab;

        private GridCell[,] _cells;

        public int Rows => rows;
        public int Cols => cols;
        public RectTransform GridContainer => gridContainer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            BuildGrid();
        }

        private void BuildGrid()
        {
            _cells = new GridCell[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridCell cell = Instantiate(cellPrefab, gridContainer);
                    cell.Init(r, c);
                    cell.name = $"Cell_{r}_{c}";
                    _cells[r, c] = cell;
                }
            }
        }

        public GridCell GetCell(int row, int col)
        {
            if (row < 0 || row >= rows || col < 0 || col >= cols) return null;
            return _cells[row, col];
        }

        public GridCell FindNearestCell(Vector2 screenPosition, Camera uiCamera, float maxSnapDistance)
        {
            GridCell nearest = null;
            float minDist = float.MaxValue;

            foreach (GridCell cell in _cells)
            {
                RectTransform cellRect = cell.RectTransform;
                Vector2 cellScreenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, cellRect.position);
                float dist = Vector2.Distance(screenPosition, cellScreenPos);

                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = cell;
                }
            }

            if (minDist > maxSnapDistance) return null;
            return nearest;
        }

        public bool CanPlaceItem(GridCell targetCell, DraggableItem item)
        {
            int r = targetCell.Row;
            int c = targetCell.Col;

            for (int cc = 0; cc < cols; cc++)
            {
                GridCell cell = _cells[r, cc];
                if (cell == targetCell) continue;
                if (!cell.IsEmpty && cell.CurrentItem != item) return false;
            }

            for (int rr = 0; rr < rows; rr++)
            {
                GridCell cell = _cells[rr, c];
                if (cell == targetCell) continue;
                if (!cell.IsEmpty && cell.CurrentItem != item) return false;
            }

            return true;
        }

        public void OnItemPlaced(GridCell cell, DraggableItem item)
        {
            cell.SetItem(item);
            ValidateRowAndCol(cell.Row, cell.Col);
        }

        public void OnItemRemoved(GridCell cell)
        {
            cell.ClearItem();
        }

        public void NotifyInvalidPlacement(int r, int c)
        {
            string log = $"[GridLog] Rejected drop at ({r}, {c}). Row {r} or Col {c} already occupied by another item. Status: Invalid";
            Debug.Log(log);

            if (FeedbackUI.Instance != null)
            {
                FeedbackUI.Instance.ShowResult(log);
            }
        }

        public void ValidateRowAndCol(int r, int c)
        {
            LineScanResult rowResult = ScanLine(GetRowCells(r));
            LineScanResult colResult = ScanLine(GetColCells(c));

            bool isValid = !rowResult.hasDuplicate && !colResult.hasDuplicate;
            string status = isValid ? "Valid" : "Invalid (Duplicate)";

            string rowSuffix = rowResult.hasDuplicate ? " (DUPLICATE!)" : (rowResult.isFull ? " (FULL)" : "");
            string colSuffix = colResult.hasDuplicate ? " (DUPLICATE!)" : (colResult.isFull ? " (FULL)" : "");

            string log = $"[GridLog] Placed item at ({r}, {c}). " +
                          $"Row {r} has {rowResult.itemCount} items{rowSuffix}. " +
                          $"Col {c} has {colResult.itemCount} items{colSuffix}. " +
                          $"Status: {status}";

            Debug.Log(log);

            if (FeedbackUI.Instance != null)
            {
                FeedbackUI.Instance.ShowResult(log);
            }
        }

        private List<GridCell> GetRowCells(int r)
        {
            var list = new List<GridCell>(cols);
            for (int c = 0; c < cols; c++) list.Add(_cells[r, c]);
            return list;
        }

        private List<GridCell> GetColCells(int c)
        {
            var list = new List<GridCell>(rows);
            for (int r = 0; r < rows; r++) list.Add(_cells[r, c]);
            return list;
        }

        private struct LineScanResult
        {
            public int itemCount;
            public bool isFull;
            public bool hasDuplicate;
        }

        private LineScanResult ScanLine(List<GridCell> line)
        {
            var idCount = new Dictionary<int, int>();
            int filled = 0;

            foreach (GridCell cell in line)
            {
                if (!cell.IsEmpty)
                {
                    filled++;
                    int id = cell.CurrentItem.ItemId;
                    idCount.TryGetValue(id, out int count);
                    idCount[id] = count + 1;
                }
            }

            bool duplicate = false;
            foreach (var kv in idCount)
            {
                if (kv.Value > 1)
                {
                    duplicate = true;
                    break;
                }
            }

            return new LineScanResult
            {
                itemCount = filled,
                isFull = filled >= line.Count,
                hasDuplicate = duplicate
            };
        }
    }
}