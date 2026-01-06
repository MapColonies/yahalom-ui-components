using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.BasicButton
{
    public class MainButton : Button
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private Image _icon;

        [Header("Icon Colors")]
        
        [SerializeField] private Color _normalIconColor;
        [SerializeField] private Color _highlightedIconColor;
        [SerializeField] private Color _pressedIconColor;
        [SerializeField] private Color _disabledIconColor;

        [Header("Text Colors")] 
        [SerializeField] private Color _normalTextColor;
        [SerializeField] private Color _highlightedTextColor;
        [SerializeField] private Color _pressedTextColor;
        [SerializeField] private Color _disabledTextColor;

        [SerializeField] private float _fadeDuration = 0.1f;

        protected override void Awake()
        {
            base.Awake();

            if (_textMesh == null)
            {
                _textMesh = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (_textMesh == null || !Application.isPlaying)
                return;

            Color targetIconColor = _normalIconColor;
            Color targetTextColor = _normalTextColor;

            switch (state)
            {
                case SelectionState.Normal:
                    targetTextColor = _normalTextColor;
                    targetIconColor = _normalIconColor;
                    break;
                case SelectionState.Highlighted:
                    targetTextColor = _highlightedTextColor;
                    targetIconColor = _highlightedIconColor;
                    break;
                case SelectionState.Pressed:
                    targetTextColor = _pressedTextColor;
                    targetIconColor = _pressedIconColor;
                    break;
                case SelectionState.Selected:
                    targetTextColor = _normalTextColor;
                    targetIconColor = _normalIconColor;
                    break;
                case SelectionState.Disabled:
                    targetTextColor = _disabledTextColor;
                    targetIconColor = _disabledIconColor;
                    break;
            }

            if (instant || _fadeDuration == 0f)
            {
                _textMesh.color = targetTextColor;
                _icon.color = targetIconColor;
            }
            else
            {
                _textMesh.CrossFadeColor(targetTextColor, _fadeDuration, true, true);
                _icon.CrossFadeColor(targetIconColor, _fadeDuration, true, true);
            }
        }
    }
}
