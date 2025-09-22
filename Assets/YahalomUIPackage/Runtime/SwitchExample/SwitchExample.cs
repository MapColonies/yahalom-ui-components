using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.SwitchExample
{
    [UxmlElement]
    public partial class SwitchExample : VisualElement
    {
        [UxmlAttribute]
        public string Text { get => _label.text; set => _label.text = value; }
        
        [UxmlAttribute]
        public bool Value { get => _value;
            set => SetValue(value);
        }

        public Action<bool> OnValueChanged { get; set; }

        private Label _label;

        private VisualElement _border;
        private VisualElement _control;

        private bool _value;

        public SwitchExample()
        {
            var styleSheet = Resources.Load<StyleSheet>("SwitchExample/SwitchExample"); 
            styleSheets.Add(styleSheet);
            
            _label = new Label("Name");
            _label.name = "Text";
            
            _border = new VisualElement();
            _border.name = "Border";
            _border.AddToClassList("switchexample-border");
            
            _control = new VisualElement();
            _control.name = "Control";
            _control.AddToClassList("switchexample-control");
            
            Add(_label);
            Add(_border);
            _border.Add(_control);

            _border.RegisterCallback<MouseDownEvent>(evt => Value = !Value);
        }

        private void SetValue(bool value)
        {
            _value = value;
            OnValueChanged?.Invoke(value);
            SetState(value);
        }

        private void SetState(bool value)
        {
            _border.EnableInClassList("switchexample-border_on", value);
            _control.EnableInClassList("switchexample-control_on", value);
        }
    }
}
