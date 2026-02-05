using System;
using TMPro;
using UnityEngine;

namespace YahalomUIPackage.Runtime.YahalomInputField
{
    public class YahalomInputField : TMP_InputField
    {
        [Header("Text Colors")] 
        [SerializeField] private Color _normalTextColor;
        [SerializeField] private Color _highlightedTextColor;
        [SerializeField] private Color _pressedTextColor;
        [SerializeField] private Color _selectedTextColor;
        [SerializeField] private Color _disabledTextColor;
        [SerializeField] private Color _errorTextColor;

        [Header("Image Colors")] 
        [SerializeField] private Color _errorImageColor;

        private Func<string, bool> _errorPredicate;
        private bool _isInErrorState;

        protected override void Awake()
        {
            base.Awake();
            
            onValueChanged.AddListener(OnValueChanged);
            onSubmit.AddListener(OnSubmit);
            onDeselect.AddListener(OnDeselect);
        }

        private void OnDestroy()
        {
            onValueChanged.RemoveListener(OnValueChanged);
            onSubmit.RemoveListener(OnSubmit);
            onDeselect.RemoveListener(OnDeselect);
        }

        private void OnDeselect(string value)
        {
            SetPlaceholderColor(_normalTextColor);

            if (IsError(value))
            {
                text = string.Empty;
            }
            else if (text != string.Empty)
            {
                SetTextColor(_selectedTextColor);
            }
        }

        private void OnSubmit(string value)
        {
            SetTextColor(value == string.Empty ? _normalTextColor : _selectedTextColor);
        }

        private void OnValueChanged(string value)
        {
            if (IsError(value))
            {
                ApplyErrorState();
            }
        }

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            if (_isInErrorState)
            {
                return;
            }

            Color textColor = _normalTextColor;

            switch (state)
            {
                case SelectionState.Disabled:
                    textColor = _disabledTextColor;
                    break;
                case SelectionState.Highlighted:
                case SelectionState.Pressed:
                case SelectionState.Selected:
                    textColor = _highlightedTextColor;
                    break;
                case SelectionState.Normal:
                    textColor = _normalTextColor;
                    break;
            }

            SetTextColor(textColor);
            SetPlaceholderColor(textColor);
        }

        private void ApplyErrorState()
        {
            _isInErrorState = true;
            SetImageColor(_errorImageColor);
        }

        private void SetTextColor(Color color)
        {
            textComponent.color = color;
        }

        private void SetPlaceholderColor(Color color)
        {
            placeholder.color = color;
        }

        private void SetImageColor(Color color)
        {
            image.color = color;
        }

        private bool IsError(string value)
        {
            return _errorPredicate != null && _errorPredicate(value);
        }

        public void SetErrorPredicate(Func<string, bool> predicate)
        {
            _errorPredicate = predicate;
        }
    }
}
