# GridMeowdokuLogic — Tài liệu kỹ thuật

> **Mục đích:** Demo/luyện tập logic kéo-thả (drag & drop) trên Unity UI kết hợp luật xếp hàng/cột kiểu Sudoku/N-Queens.
> **Namespace:** `GridDragDrop`
> **Scene:** `Scenes/Tan.unity`

---

## 1. Tổng quan kiến trúc

Hệ thống chia thành **3 lớp trách nhiệm rõ ràng**:

| Lớp | Script | Vai trò |
|---|---|---|
| **Logic / Data** | `GridManager` | Nguồn dữ liệu duy nhất, luật chơi, validate |
| **Data cell** | `GridCell` | Lưu tọa độ + item hiện tại, không validate |
| **Input / View** | `DraggableItem` | Xử lý kéo thả, gọi API của GridManager |
| **View** | `FeedbackUI` | Hiển thị kết quả lên TextMeshPro |

---

## 2. Mô tả từng script

### GridManager.cs — Logic trung tâm

**Singleton** (`GridManager.Instance`) để các script khác truy cập không cần SerializeField reference.

#### Khởi tạo lưới (BuildGrid)
- Spawn `rows x cols` cell prefab vào `gridContainer` (có `GridLayoutGroup`).
- `GridLayoutGroup` tự lo scale & vị trí — script không cần tính RectTransform thủ công.
- Mỗi cell được đặt tên `Cell_r_c` và gán tọa độ qua `cell.Init(r, c)`.

#### API public cho DraggableItem

| Phương thức | Mô tả |
|---|---|
| `FindNearestCell(screenPos, camera, maxDist)` | Duyệt toàn bộ cell, tìm cái gần screenPos nhất trong bán kính maxDist pixel |
| `CanPlaceItem(targetCell, item)` | Quét hàng + cột: nếu đã có bất kỳ item nào khác thì từ chối |
| `OnItemPlaced(cell, item)` | Gán item vào cell rồi gọi ValidateRowAndCol |
| `OnItemRemoved(cell)` | Xóa item khỏi cell khi item được kéo đi chỗ khác |
| `NotifyInvalidPlacement(r, c)` | Log + hiển thị thông báo khi thả bị từ chối |

#### Validate (ValidateRowAndCol + ScanLine)
Sau mỗi lần đặt thành công, quét lại hàng và cột vừa thay đổi:
- Đếm số ô có item.
- Kiểm tra trùng ItemId (cùng Id xuất hiện 2 lần = vi phạm kiểu Sudoku).
- Kiểm tra Full (số item = độ dài hàng/cột).
- Log format: `[GridLog] Placed item at (r, c). Row r has N items[suffix]. Col c has N items[suffix]. Status: Valid/Invalid`

---

### GridCell.cs — Data của 1 ô

- Lưu `Row`, `Col` (readonly sau Init).
- Lưu `CurrentItem` (reference tới DraggableItem đang đứng trên ô).
- Property `IsEmpty`, `RectTransform` (lazy-init).
- Không có bất kỳ logic validate nào — đúng nguyên tắc Single Responsibility.

---

### DraggableItem.cs — Input và kéo thả

Implement 3 interface chuẩn Unity EventSystem:

| Interface | Khi nào kích hoạt |
|---|---|
| `IBeginDragHandler` | Bắt đầu kéo |
| `IDragHandler` | Trong lúc kéo |
| `IEndDragHandler` | Thả ra |

Dùng EventSystem interfaces thay vì Update() vì tự động hỗ trợ cả Mouse (PC) và Touch (Mobile).

#### Luồng kéo thả

```
BeginDrag
  ├── Lưu _originalParent & _originalAnchoredPos
  ├── SetParent -> rootCanvas (vẽ đè lên mọi UI khác)
  └── blocksRaycasts = false

Drag
  └── anchoredPosition += delta / scaleFactor

EndDrag
  ├── blocksRaycasts = true
  ├── FindNearestCell(...)
  │   ├── hợp lệ -> PlaceInCell
  │   │     ├── OnItemRemoved(CurrentCell cũ) nếu có
  │   │     ├── SetParent -> cell.transform
  │   │     ├── anchoredPosition = (0,0)  <- snap vào tâm cell
  │   │     └── OnItemPlaced -> ValidateRowAndCol
  │   └── không hợp lệ -> ResetToOriginalPosition
  └── NotifyInvalidPlacement nếu CanPlaceItem = false
```

**Chi tiết kỹ thuật:**
- `delta / scaleFactor`: đảm bảo di chuyển đúng pixel khi Canvas dùng Scale With Screen Size.
- `uiCamera = null` khi Canvas ở Screen Space - Overlay: yêu cầu của RectTransformUtility.WorldToScreenPoint.
- `blocksRaycasts = false` trong lúc kéo: cho phép raycast xuyên qua item để hit cell phía dưới.

---

### FeedbackUI.cs — View hiển thị kết quả

- Singleton đơn giản (không có guard destroy).
- Nhận string message từ GridManager và gán thẳng vào TMP_Text.
- Mục đích: xem log kết quả trực tiếp trên thiết bị thật mà không cần cắm cáp xem Console.

---

## 3. Luật chơi

| Luật | Mô tả |
|---|---|
| **1 item / hàng** | Mỗi hàng chỉ chứa tối đa 1 item (bất kể ItemId) |
| **1 item / cột** | Mỗi cột chỉ chứa tối đa 1 item |
| **Không trùng ItemId** | Trong 1 hàng/cột, không được có 2 item cùng ItemId |

> Lưu ý: Luật "1 item/hàng-cột" được enforce TRƯỚC KHI đặt (CanPlaceItem — block).
> Luật "không trùng ItemId" chỉ được detect và log SAU KHI đặt (ValidateRowAndCol — không block).
> Đây là thiết kế học thuật; production nên hợp nhất 2 luật vào 1 bước kiểm tra.

---

## 4. Nhận xét code

### Điểm tốt

- Phân tầng rõ ràng: GridManager (logic) <-> GridCell (data) <-> DraggableItem (input) <-> FeedbackUI (view). Không có dependency vòng tròn.
- Single Source of Truth: `_cells[,]` là mảng dữ liệu duy nhất.
- EventSystem interfaces: cross-platform ngay từ đầu.
- Canvas scaling chuẩn: dùng `delta / scaleFactor` và `RectTransformUtility`.
- GridLayoutGroup delegation: không tự tính vị trí cell, tận dụng Unity layout system.
- `[RequireComponent]`: đảm bảo RectTransform và CanvasGroup luôn có mặt.
- Lazy-init RectTransform trong GridCell: tránh GetComponent gọi lặp.

### Điểm cần cải thiện (cho production)

| Vấn đề | Hiện trạng | Gợi ý |
|---|---|---|
| FeedbackUI Singleton không an toàn | Không có guard, object thứ 2 sẽ ghi đè Instance | Thêm destroy guard như GridManager |
| 2 luật không đồng nhất | CanPlaceItem block theo số lượng; ScanLine detect theo ItemId | Hợp nhất cả 2 luật vào CanPlaceItem |
| FindNearestCell O(n) duyệt toàn bộ | Ổn với 9x9, nhưng không scale tốt | Dùng spatial partitioning cho lưới lớn |
| Snap distance dạng screen pixel | Phụ thuộc độ phân giải | Dùng normalized hoặc tính theo cell size |
| Không có animation | Snap ngay tức thì | Thêm DOTween/LeanTween |
| Không có undo | Reset về gốc khi thả sai | Có thể thêm history stack |

---

## 5. Cấu trúc folder

```
GridMeowdokuLogic/
├── Assets/
│   └── Prefabs/          <- Cell prefab & Item prefab
├── Material/             <- Material cho cell/item
├── Scenes/
│   └── Tan.unity         <- Scene demo
├── Scripts/
│   ├── GridManager.cs    <- Logic trung tâm
│   ├── GridCell.cs       <- Data 1 ô
│   ├── DraggableItem.cs  <- Input kéo thả
│   └── FeedbackUI.cs     <- Hiển thị kết quả
└── README.md             <- File này
```

---

## 6. Setup trong Unity Editor

1. Tạo **Canvas** (Screen Space Overlay hoặc Camera).
2. Trong Canvas, tạo **Panel** gắn `GridLayoutGroup` -> kéo vào `Grid Container` của GridManager.
3. Tạo **Cell Prefab**: Image + GridCell.cs -> kéo vào `Cell Prefab`.
4. Tạo **Item GameObject**: Image + RectTransform + CanvasGroup + DraggableItem.cs -> set `ItemId`.
5. Tạo **Text (TMP)** trong Canvas -> gắn FeedbackUI.cs -> kéo TMP vào `Result Text`.
6. Tạo **Empty GameObject** -> gắn GridManager.cs -> điền rows, cols, Grid Container, Cell Prefab.

---

*Tài liệu được tổng hợp từ source code trong `GridMeowdokuLogic/Scripts/`.*
