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
        [SerializeField] private RectTransform _arrow;

        [SerializeField] private float _fadeInDuration = 0.2f;
        [SerializeField] private float _fadeOutDuration = 0.15f;

        [SerializeField] private float _arrowEdgePadding = 8f;

        private readonly Vector3[] _corners = new Vector3[4];

        private RectTransform _rect;
        private CanvasGroup _canvasGroup;
        private Canvas _rootCanvas;
        private bool _rootCanvasResolved;
        private Object _owner;
        private float _targetAlpha;

        private bool _arrowDefaultsSaved;
        private Vector2 _arrowDefaultAnchor;
        private Vector2 _arrowDefaultPosition;
        private Quaternion _arrowDefaultRotation;

        private RectTransform Rect => _rect != null ? _rect : _rect = (RectTransform)transform;
        private CanvasGroup Group => _canvasGroup != null ? _canvasGroup : _canvasGroup = GetComponent<CanvasGroup>();

        private void Awake()
        {
            Group.blocksRaycasts = false;
            Group.interactable = false;
            Group.alpha = 0f;
            SaveArrowDefaults();
        }

        private void Update()
        {
            float alpha = Group.alpha;
            if (alpha == _targetAlpha) return;

            float duration = _targetAlpha > alpha ? _fadeInDuration : _fadeOutDuration;
            Group.alpha = duration > 0f
                ? Mathf.MoveTowards(alpha, _targetAlpha, Time.unscaledDeltaTime / duration)
                : _targetAlpha;
        }

        private void OnTransformParentChanged()
        {
            _rootCanvasResolved = false;
        }

        public void Show(Object owner, RectTransform target, string title, string text, float gap)
        {
            _owner = owner;
            SetContent(title, text);
            Place(target, gap);
            _targetAlpha = 1f;
        }

        public void Refresh(Object owner, RectTransform target, string title, string text, float gap)
        {
            if (_owner != owner) return;

            SetContent(title, text);
            Place(target, gap);
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

        private void Place(RectTransform target, float gap)
        {
            target.GetWorldCorners(_corners);
            Vector3 topCenter = (_corners[1] + _corners[2]) * 0.5f;
            Vector3 bottomCenter = (_corners[0] + _corners[3]) * 0.5f;
            Vector3 offset = target.up * (gap * Rect.lossyScale.y);

            PlaceAt(topCenter + offset, true);

            if (!IsScreenSpaceOverlay()) return;

            Rect.GetWorldCorners(_corners);
            if (_corners[1].y > Screen.height)
                PlaceAt(bottomCenter - offset, false);

            KeepInsideScreenHorizontally();
        }

        private void PlaceAt(Vector3 position, bool above)
        {
            Rect.pivot = new Vector2(0.5f, above ? 0f : 1f);
            Rect.position = position;
            SetArrowSide(above);
        }

        private void KeepInsideScreenHorizontally()
        {
            Rect.GetWorldCorners(_corners);

            float shift = 0f;
            if (_corners[0].x < 0f)
                shift = -_corners[0].x;
            else if (_corners[2].x > Screen.width)
                shift = Screen.width - _corners[2].x;

            if (shift == 0f) return;

            Rect.position += new Vector3(shift, 0f, 0f);

            if (_arrow == null) return;

            float maxOffset = Mathf.Max(0f, Rect.rect.width * 0.5f - _arrow.rect.width * 0.5f - _arrowEdgePadding);
            float arrowX = Mathf.Clamp(-shift / Rect.lossyScale.x, -maxOffset, maxOffset);
            _arrow.anchoredPosition = new Vector2(arrowX, _arrow.anchoredPosition.y);
        }

        private bool IsScreenSpaceOverlay()
        {
            if (!_rootCanvasResolved)
            {
                Canvas canvas = GetComponentInParent<Canvas>();
                _rootCanvas = canvas != null ? canvas.rootCanvas : null;
                _rootCanvasResolved = true;
            }

            return _rootCanvas != null && _rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay;
        }

        private void SaveArrowDefaults()
        {
            if (_arrowDefaultsSaved || _arrow == null) return;

            _arrowDefaultAnchor = _arrow.anchorMin;
            _arrowDefaultPosition = _arrow.anchoredPosition;
            _arrowDefaultRotation = _arrow.localRotation;
            _arrowDefaultsSaved = true;
        }

        private void SetArrowSide(bool above)
        {
            SaveArrowDefaults();
            if (!_arrowDefaultsSaved) return;

            Vector2 anchor = above ? _arrowDefaultAnchor : new Vector2(_arrowDefaultAnchor.x, 1f - _arrowDefaultAnchor.y);
            _arrow.anchorMin = anchor;
            _arrow.anchorMax = anchor;
            _arrow.anchoredPosition = above
                ? _arrowDefaultPosition
                : new Vector2(_arrowDefaultPosition.x, -_arrowDefaultPosition.y);
            _arrow.localRotation = above
                ? _arrowDefaultRotation
                : _arrowDefaultRotation * Quaternion.Euler(0f, 0f, 180f);
        }
    }
}
