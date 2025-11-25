using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicRadio
{
    [UxmlElement]
    public partial class BasicRadio : VisualElement
    {
        [UxmlAttribute]
        public bool Selected
        {
            get => _selected;
            set => SetSelected(value, true);
        }

        [UxmlAttribute]
        public bool Disabled
        {
            get => _disabled;
            set => SetDisabled(value);
        }

        [UxmlAttribute]
        public string Text
        {
            get => _label?.text;
            set
            {
                if (_label != null)
                    _label.text = value;
            }
        }

        public Action<bool> OnSelectedChanged { get; set; }

        private bool _selected;
        private bool _disabled;

        private VisualElement _outer;
        private VisualElement _inner;
        private Label _label;

        public BasicRadio()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicRadio/BasicRadio");
            if (styleSheet != null)
                styleSheets.Add(styleSheet);
            else
                Debug.LogError("[BasicRadio] missing USS at Assets/Resources/BasicRadio/BasicRadio.uss");

            AddToClassList("basic-radio");

            _outer = new VisualElement();
            _outer.AddToClassList("basic-radio__outer");

            _inner = new VisualElement();
            _inner.AddToClassList("basic-radio__inner");
            _outer.Add(_inner);

            _label = new Label();
            _label.AddToClassList("basic-radio__label");

            Add(_outer);
            Add(_label);

            RegisterCallback<ClickEvent>(OnClicked);

            UpdateVisualState();
        }

        private void OnClicked(ClickEvent evt)
        {
            if (_disabled)
                return;

            if (!_selected)
                SetSelected(true, true);
        }

        internal void SetSelectedInternal(bool value) => SetSelected(value, false);

        private void SetSelected(bool value, bool invokeCallback)
        {
            if (_selected == value)
                return;

            _selected = value;
            UpdateVisualState();

            if (invokeCallback)
                OnSelectedChanged?.Invoke(_selected);
        }

        private void SetDisabled(bool value)
        {
            if (_disabled == value)
                return;

            _disabled = value;
            pickingMode = _disabled ? PickingMode.Ignore : PickingMode.Position;
            UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            EnableInClassList("basic-radio--selected", _selected);
            EnableInClassList("basic-radio--disabled", _disabled);
        }
    }
}
