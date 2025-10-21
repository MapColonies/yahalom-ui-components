using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

// Namespace updated
namespace YahalomUIPackage.Runtime.BasicDropdown
{
    [UxmlElement]
    // Class name updated
    public partial class BasicDropdown : VisualElement
    {
        /// <summary>
        /// Fires when the selected value is changed by the user.
        /// </summary>
        public Action<string> OnValueChanged { get; set; }

        /// <summary>
        /// Sets the available options from a comma-separated string.
        /// (e.g., "Option 1, Option 2, Option 3")
        /// </summary>
        [UxmlAttribute]
        public string options
        {
            // We add a 'get' accessor, though it's not strictly needed
            get => string.Join(", ", _optionsList); 
            
            // This is the important part
            set
            {
                // Check if the value from UXML is null or empty
                if (string.IsNullOrEmpty(value))
                {
                    // If it is, just set an empty list of options
                    SetOptions(new List<string>());
                }
                else
                {
                    // Otherwise, split the string as before
                    SetOptions(value.Split(',').Select(s => s.Trim()).ToList());
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently selected value.
        /// </summary>
        [UxmlAttribute]
        public string Value
        {
            get => _value;
            set => SetValue(value, false); // Set value without firing event
        }

        private List<string> _optionsList = new List<string>();
        private string _value;
        private bool _isOpen = false;

        // --- Internal Visual Elements ---
        private VisualElement _button;
        private Label _label;
        private VisualElement _icon;
        private VisualElement _optionsContainer;
        
        // This default constructor is required for UXML instantiation
        // Constructor name updated
        public BasicDropdown()
        {
            // Load stylesheet (path updated)
            var styleSheet = Resources.Load<StyleSheet>("BasicDropdown/BasicDropdown");
            styleSheets.Add(styleSheet);
            
            // Set base class for the component (class name updated)
            AddToClassList("basic-dropdown");

            // --- Create Element Hierarchy ---
            _button = new VisualElement();
            _button.name = "Button";
            // USS class name updated
            _button.AddToClassList("basic-dropdown-button");

            _label = new Label("Select...");
            _label.name = "Label";
            // USS class name updated
            _label.AddToClassList("basic-dropdown-label");

            _icon = new VisualElement();
            _icon.name = "Icon";
            // USS class name updated
            _icon.AddToClassList("basic-dropdown-icon");
            
            _optionsContainer = new VisualElement();
            _optionsContainer.name = "OptionsContainer";
            // USS class name updated
            _optionsContainer.AddToClassList("basic-dropdown-options");
            _optionsContainer.style.display = DisplayStyle.None; // Hide by default

            // --- Assemble Hierarchy ---
            _button.Add(_label);
            _button.Add(_icon);
            Add(_button);
            Add(_optionsContainer);

            // --- Register Callbacks ---
            _button.RegisterCallback<MouseDownEvent>(evt => ToggleDropdown());
            
            // Allow this element to be focused to detect "click outside"
            focusable = true;
            RegisterCallback<FocusOutEvent>(evt => CloseDropdown());
        }

        /// <summary>
        /// Populates the dropdown with a list of string options.
        /// </summary>
        public void SetOptions(List<string> options)
        {
            _optionsList = options;
            _optionsContainer.Clear();

            foreach (var option in _optionsList)
            {
                Label optionLabel = new Label(option);
                // USS class name updated
                optionLabel.AddToClassList("basic-dropdown-option");
                
                // Register callback for when an option is clicked
                optionLabel.RegisterCallback<MouseDownEvent>(evt =>
                {
                    SetValue(option, true);
                });
                
                _optionsContainer.Add(optionLabel);
            }

            // Set default value if none is set, or update label if value was set via UXML
            if (string.IsNullOrEmpty(_value) && _optionsList.Count > 0)
            {
                SetValue(_optionsList[0], false);
            }
            else
            {
                _label.text = _value;
            }
        }

        /// <summary>
        /// Sets the current value of the dropdown.
        /// </summary>
        /// <param name="value">The new value to set.</param>
        /// <param name="notify">If true, invokes the OnValueChanged event.</param>
        public void SetValue(string value, bool notify = true)
        {
            if (_value == value) return;

            _value = value;
            _label.text = value;
            CloseDropdown();

            if (notify)
            {
                OnValueChanged?.Invoke(value);
            }
        }
        
        private void ToggleDropdown()
        {
            if (_isOpen)
            {
                CloseDropdown();
            }
            else
            {
                OpenDropdown();
            }
        }
        
        private void OpenDropdown()
        {
            if (_isOpen) return;
            _isOpen = true;
            _optionsContainer.style.display = DisplayStyle.Flex;
            // USS class name updated
            _icon.EnableInClassList("basic-dropdown-icon--open", true);
            this.Focus(); // Focus to receive FocusOutEvent
        }

        private void CloseDropdown()
        {
            if (!_isOpen) return;
            _isOpen = false;
            _optionsContainer.style.display = DisplayStyle.None;
            // USS class name updated
            _icon.EnableInClassList("basic-dropdown-icon--open", false);
        }
    }
}
