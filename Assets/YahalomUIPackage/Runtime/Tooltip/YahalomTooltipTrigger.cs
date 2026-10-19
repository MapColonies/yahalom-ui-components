using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YahalomUIPackage.Runtime.Tooltip
{
    public class YahalomTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private YahalomTooltip _tooltip;
        [SerializeField] private float _gap = 8f;

        private string _title = string.Empty;
        private string _description = string.Empty;
        private Func<string> _descriptionSource;
        private bool _isShowing;
        private bool _warnedMissingTooltip;

        public string Title
        {
            get => _title;
            set
            {
                _title = value ?? string.Empty;
                RefreshIfShowing();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value ?? string.Empty;
                RefreshIfShowing();
            }
        }

        public void SetDescriptionSource(Func<string> source)
        {
            _descriptionSource = source;
        }

        public void SetTooltip(YahalomTooltip tooltip)
        {
            if (_tooltip == tooltip) return;

            Hide();
            _tooltip = tooltip;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tooltip == null)
            {
                WarnMissingTooltip();
                return;
            }

            if (_descriptionSource != null)
                _description = _descriptionSource() ?? string.Empty;

            if (string.IsNullOrEmpty(_description) && string.IsNullOrEmpty(_title)) return;

            _isShowing = true;
            _tooltip.Show(this, (RectTransform)transform, _title, _description, _gap);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        private void LateUpdate()
        {
            if (!_isShowing || _descriptionSource == null) return;

            string description = _descriptionSource() ?? string.Empty;
            if (description != _description)
                Description = description;
        }

        private void OnDisable()
        {
            Hide();
        }

        private void Hide()
        {
            if (!_isShowing) return;

            _isShowing = false;
            if (_tooltip != null)
                _tooltip.Hide(this);
        }

        private void RefreshIfShowing()
        {
            if (_isShowing && _tooltip != null)
                _tooltip.Refresh(this, (RectTransform)transform, _title, _description, _gap);
        }

        private void WarnMissingTooltip()
        {
            if (_warnedMissingTooltip) return;

            _warnedMissingTooltip = true;
            Debug.LogWarning($"{nameof(YahalomTooltipTrigger)} on '{name}' has no tooltip assigned.", this);
        }
    }
}
