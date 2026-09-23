using UnityEngine;

namespace GridDragDrop
{
    public class GridCell : MonoBehaviour
    {
        public int Row { get; private set; }
        public int Col { get; private set; }

        public DraggableItem CurrentItem { get; private set; }

        public bool IsEmpty => CurrentItem == null;

        private RectTransform _rectTransform;
        public RectTransform RectTransform =>
            _rectTransform != null ? _rectTransform : (_rectTransform = GetComponent<RectTransform>());

        public void Init(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public void SetItem(DraggableItem item)
        {
            CurrentItem = item;
        }

        public void ClearItem()
        {
            CurrentItem = null;
        }
    }
}
