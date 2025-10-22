using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicToggle
{
    [UxmlElement]
    public partial class BasicToggle : VisualElement
    {
        [UxmlAttribute]
        public string Text
        {
            get => _label.text;
            set => _label.text = value;
        }

        [UxmlAttribute]
        public bool Value
        {
            get => _value;
            set => SetValue(value);
        }

        public Action<bool> OnValueChanged { get; set; }

        private Label _label;
        private VisualElement _border;
        private VisualElement _control;
        private bool _value;

        public BasicToggle()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicToggle/BasicToggle");
            styleSheets.Add(styleSheet);

            AddToClassList("basic-toggle");

            _label = new Label("Toggle");
            _label.name = "Text";

            _border = new VisualElement();
            _border.name = "Border";
            _border.AddToClassList("basic-toggle-border");

            _control = new VisualElement();
            _control.name = "Control";
            _control.AddToClassList("basic-toggle-control");

            Add(_label);
            Add(_border);
            _border.Add(_control);
            _border.RegisterCallback<MouseDownEvent>(evt => Value = !Value);
        }

        private void SetValue(bool value)
        {
            if (_value == value) return;

            _value = value;
            OnValueChanged?.Invoke(value);
            SetState(value);
        }

        private void SetState(bool value)
        {
            _border.EnableInClassList("basic-toggle-border--on", value);
            _control.EnableInClassList("basic-toggle-control--on", value);
        }
    }
}
