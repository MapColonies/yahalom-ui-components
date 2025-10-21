using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicToggle
{
    [UxmlElement]
    public partial class BasicToggle : VisualElement
    {
        /// <summary>
        /// A label for the toggle.
        /// </summary>
        [UxmlAttribute]
        public string Text { get => _label.text; set => _label.text = value; }
        
        /// <summary>
        /// The boolean on/off state of the toggle.
        /// </summary>
        [UxmlAttribute]
        public bool Value { get => _value;
            set => SetValue(value);
        }

        /// <summary>
        /// Fires when the value (on/off state) is changed by the user.
        /// </summary>
        public Action<bool> OnValueChanged { get; set; }

        private Label _label;
        private VisualElement _border;
        private VisualElement _control;
        private bool _value;

        // This default constructor is required for UXML instantiation
        public BasicToggle()
        {
            // Load stylesheet
            var styleSheet = Resources.Load<StyleSheet>("BasicToggle/BasicToggle"); 
            styleSheets.Add(styleSheet);
            
            // Set base class for the component
            AddToClassList("basic-toggle");

            // --- Create Element Hierarchy ---
            _label = new Label("Toggle");
            _label.name = "Text";
            
            _border = new VisualElement();
            _border.name = "Border";
            _border.AddToClassList("basic-toggle-border");
            
            _control = new VisualElement();
            _control.name = "Control";
            _control.AddToClassList("basic-toggle-control");
            
            // --- Assemble Hierarchy ---
            Add(_label);
            Add(_border);
            _border.Add(_control);

            // --- Register Callbacks ---
            // Register callback on the border element
            _border.RegisterCallback<MouseDownEvent>(evt => Value = !Value);
        }

        private void SetValue(bool value)
        {
            // Prevent redundant sets
            if (_value == value) return;
            
            _value = value;
            OnValueChanged?.Invoke(value);
            SetState(value);
        }

        private void SetState(bool value)
        {
            // Use EnableInClassList to add/remove the --on modifier classes
            _border.EnableInClassList("basic-toggle-border--on", value);
            _control.EnableInClassList("basic-toggle-control--on", value);
        }
    }
}