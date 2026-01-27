using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.YahalomButton
{
    public class YahalomButton : Button
    {
        [SerializeField] private TextMeshProUGUI _textMesh;
        [SerializeField] private Image _icon;

        [Header("Icons")]
        [SerializeField] private Color _normalIconColor;
        [SerializeField] private Color _highlightedIconColor;
        [SerializeField] private Color _pressedIconColor;
        [SerializeField] private Color _selectedIconColor;
        [SerializeField] private Color _disabledIconColor;

        [Header("Text Colors")] 
        [SerializeField] private Color _normalTextColor;
        [SerializeField] private Color _highlightedTextColor;
        [SerializeField] private Color _pressedTextColor;
        [SerializeField] private Color _selectedTextColor;
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

            Color targetIcon = _normalIconColor;
            Color targetTextColor = _normalTextColor;

            switch (state)
            {
                case SelectionState.Normal:
                    targetTextColor = _normalTextColor;
                    targetIcon = _normalIconColor;
                    break;
                case SelectionState.Highlighted:
                    targetTextColor = _highlightedTextColor;
                    targetIcon = _highlightedIconColor;
                    break;
                case SelectionState.Pressed:
                    targetTextColor = _pressedTextColor;
                    targetIcon = _pressedIconColor;
                    break;
                case SelectionState.Selected:
                    targetTextColor = _selectedTextColor;
                    targetIcon = _selectedIconColor;
                    break;
                case SelectionState.Disabled:
                    targetTextColor = _disabledTextColor;
                    targetIcon = _disabledIconColor;
                    break;
            }
            
            _icon.color = targetIcon;

            if (instant || _fadeDuration == 0f)
            {
                _textMesh.color = targetTextColor;
            }
            else
            {
                _textMesh.CrossFadeColor(targetTextColor, _fadeDuration, true, true);
            }
        }
    }
}
