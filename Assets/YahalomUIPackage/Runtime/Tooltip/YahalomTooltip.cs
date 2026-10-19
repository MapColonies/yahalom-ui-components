using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.Tooltip
{
    [RequireComponent(typeof(CanvasGroup))]
    public class YahalomTooltip : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private TMP_Text _text;

        [SerializeField] private float _fadeInDuration = 0.2f;
        [SerializeField] private float _fadeOutDuration = 0.15f;

        private RectTransform _rect;
        private CanvasGroup _canvasGroup;
        private Object _owner;
        private float _targetAlpha;

        private RectTransform Rect => _rect != null ? _rect : _rect = (RectTransform)transform;
        private CanvasGroup Group => _canvasGroup != null ? _canvasGroup : _canvasGroup = GetComponent<CanvasGroup>();

        private void Awake()
        {
            Group.blocksRaycasts = false;
            Group.interactable = false;
            Group.alpha = 0f;
        }

        private void Update()
        {
            float duration = _targetAlpha > Group.alpha ? _fadeInDuration : _fadeOutDuration;
            Group.alpha = duration > 0f
                ? Mathf.MoveTowards(Group.alpha, _targetAlpha, Time.unscaledDeltaTime / duration)
                : _targetAlpha;
        }

        public void Show(Object owner, RectTransform target, string title, string text, float gap)
        {
            _owner = owner;
            SetContent(title, text);
            PlaceAbove(target, gap);
            _targetAlpha = 1f;
        }

        public void Refresh(Object owner, RectTransform target, string title, string text, float gap)
        {
            if (_owner != owner) return;

            SetContent(title, text);
            PlaceAbove(target, gap);
        }

        public void Hide(Object owner)
        {
            if (_owner != owner) return;

            _owner = null;
            _targetAlpha = 0f;
        }

        private void SetContent(string title, string text)
        {
            bool hasTitle = !string.IsNullOrEmpty(title);
            _title.gameObject.SetActive(hasTitle);
            if (hasTitle)
                _title.text = title;
            _text.text = text ?? string.Empty;

            LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
        }

        private void PlaceAbove(RectTransform target, float gap)
        {
            Rect.pivot = new Vector2(0.5f, 0f);

            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners);
            Vector3 topCenter = (corners[1] + corners[2]) * 0.5f;
            Rect.position = topCenter + target.up * (gap * Rect.lossyScale.y);

            KeepInsideScreen();
        }

        private void KeepInsideScreen()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null || canvas.rootCanvas.renderMode != RenderMode.ScreenSpaceOverlay) return;

            Vector3[] corners = new Vector3[4];
            Rect.GetWorldCorners(corners);

            float shift = 0f;
            if (corners[0].x < 0f)
                shift = -corners[0].x;
            else if (corners[2].x > Screen.width)
                shift = Screen.width - corners[2].x;

            if (shift != 0f)
                Rect.position += new Vector3(shift, 0f, 0f);
        }
    }
}
