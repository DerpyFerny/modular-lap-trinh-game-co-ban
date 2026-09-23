using TMPro;
using UnityEngine;

namespace GridDragDrop
{
    public class FeedbackUI : MonoBehaviour
    {
        public static FeedbackUI Instance { get; private set; }

        [Tooltip("Text (TMP) trên Canvas dùng để hiển thị log kết quả kiểm tra")]
        [SerializeField] private TMP_Text resultText;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowResult(string message)
        {
            if (resultText != null)
            {
                resultText.text = message;
            }
        }
    }
}
