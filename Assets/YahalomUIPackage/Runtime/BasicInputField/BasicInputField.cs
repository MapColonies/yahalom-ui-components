using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicInputField
{
    [UxmlElement]
    public partial class BasicInputField : VisualElement
    {
        #region UXML Factory
        // Provides default values for UXML attributes to prevent
        // null-reference errors in the UI Builder.
        public new class UxmlFactory : UxmlFactory<BasicInputField, UxmlTraits> {}
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            // Define the attributes and their default values
            UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription { name = "text", defaultValue = "Input Label" };
            UxmlStringAttributeDescription m_Value = new UxmlStringAttributeDescription { name = "value", defaultValue = "" };
            UxmlStringAttributeDescription m_Placeholder = new UxmlStringAttributeDescription { name = "placeholder", defaultValue = "Enter text..." };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as BasicInputField;

                // Read values from UXML, using defaults if not present
                ate.Text = m_Text.GetValueFromBag(bag, cc);
                ate.Value = m_Value.GetValueFromBag(bag, cc);
                
                // This will now call our new 'set' accessor
                ate.Placeholder = m_Placeholder.GetValueFromBag(bag, cc);
            }
        }
        #endregion

        /// <summary>
        /// Fires when the text value is changed by the user.
        /// </summary>
        public Action<string> OnValueChanged { get; set; }

        /// <summary>
        /// The text for the label above the input field.
        /// </summary>
        [UxmlAttribute("text")]
        public string Text { get => _label.text; set => _label.text = value; }

        /// <summary>
        /// The current text value in the input field.
        /// </summary>
        [UxmlAttribute("value")]
        public string Value { get => _textField.value; set => _textField.value = value; }

        /// <summary>
        /// The placeholder text shown when the input field is empty.
        /// </summary>
        [UxmlAttribute("placeholder")]
        public string Placeholder
        {
            // Store the placeholder text ourselves
            get => _placeholder;
            set
            {
                _placeholder = value;
                // Find the internal element and set its text
                if (_placeholderElement != null)
                {
                    _placeholderElement.text = _placeholder;
                }
            }
        }


        private Label _label;
        private TextField _textField;
        
        // --- ADDED ---
        private TextElement _placeholderElement;
        private string _placeholder;
        // -------------
        
        public BasicInputField()
        {
            // Load stylesheet
            var styleSheet = Resources.Load<StyleSheet>("BasicInputField/BasicInputField");
            styleSheets.Add(styleSheet);
            
            // Set base class
            AddToClassList("basic-input-field");

            // --- Create Element Hierarchy ---
            _label = new Label();
            _label.AddToClassList("basic-input-field-label");
            
            _textField = new TextField();
            _textField.AddToClassList("basic-input-field-input");
            
            // --- MODIFIED ---
            // Find the internal placeholder element *inside* the TextField
            _placeholderElement = _textField.Q<TextElement>(className: "unity-placeholder");
            // ----------------
            
            // --- Assemble Hierarchy ---
            Add(_label);
            Add(_textField);

            // --- Register Callbacks ---
            // Propagate the TextField's value change event to our own event
            _textField.RegisterValueChangedCallback(OnInternalValueChanged);
        }

        private void OnInternalValueChanged(ChangeEvent<string> evt)
        {
            // Fire our public OnValueChanged event
            OnValueChanged?.Invoke(evt.newValue);
        }
    }
}
