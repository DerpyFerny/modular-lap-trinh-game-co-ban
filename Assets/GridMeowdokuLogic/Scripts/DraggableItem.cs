using UnityEngine;
using UnityEngine.EventSystems;

namespace GridDragDrop
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Item Data")]
        [Tooltip("ID/Type của item, dùng để so sánh trùng lặp kiểu Sudoku trong GridManager")]
        [SerializeField] private int itemId;
        public int ItemId => itemId;

        [Header("Drag Config")]
        [Tooltip("Khoảng cách tối đa (pixel màn hình) để chấp nhận snap vào 1 cell")]
        [SerializeField] private float snapDistance = 100f;

        [Tooltip("Canvas gốc chứa toàn bộ UI. Nếu để trống sẽ tự tìm ở Awake.")]
        [SerializeField] private Canvas rootCanvas;

        private RectTransform _rect;
        private CanvasGroup _canvasGroup;

        private Transform _originalParent;
        private Vector2 _originalAnchoredPos;

        public GridCell CurrentCell { get; private set; }

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();

            if (rootCanvas == null)
                rootCanvas = GetComponentInParent<Canvas>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            _originalAnchoredPos = _rect.anchoredPosition;

            transform.SetParent(rootCanvas.transform, true);
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rect.anchoredPosition += eventData.delta / rootCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;

            Camera uiCamera = rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : rootCanvas.worldCamera;

            GridCell targetCell = GridManager.Instance.FindNearestCell(eventData.position, uiCamera, snapDistance);

            bool placed = false;

            if (targetCell != null && targetCell.IsEmpty)
            {
                if (GridManager.Instance.CanPlaceItem(targetCell, this))
                {
                    PlaceInCell(targetCell);
                    placed = true;
                }
                else
                {
                    GridManager.Instance.NotifyInvalidPlacement(targetCell.Row, targetCell.Col);
                }
            }

            if (!placed)
            {
                ResetToOriginalPosition();
            }
        }

        private void PlaceInCell(GridCell cell)
        {
            if (CurrentCell != null)
            {
                GridManager.Instance.OnItemRemoved(CurrentCell);
            }

            transform.SetParent(cell.transform, false);
            _rect.anchoredPosition = Vector2.zero;

            CurrentCell = cell;
            GridManager.Instance.OnItemPlaced(cell, this);
        }

        private void ResetToOriginalPosition()
        {
            transform.SetParent(_originalParent, false);
            _rect.anchoredPosition = _originalAnchoredPos;
        }
    }
}