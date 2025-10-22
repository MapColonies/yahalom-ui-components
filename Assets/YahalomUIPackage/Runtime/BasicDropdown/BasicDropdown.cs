using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicDropdown
{
    [UxmlElement]
    public partial class BasicDropdown : VisualElement
    {
        public Action<string> OnValueChanged { get; set; }

        [UxmlAttribute]
        public string options
        {
            get => string.Join(", ", _optionsList);

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    SetOptions(new List<string>());
                }
                else
                {
                    SetOptions(value.Split(',').Select(s => s.Trim()).ToList());
                }
            }
        }

        [UxmlAttribute]
        public string Value
        {
            get => _value;
            set => SetValue(value, false);
        }

        private List<string> _optionsList = new List<string>();
        private string _value;
        private bool _isOpen;

        private VisualElement _button;
        private Label _label;
        private VisualElement _icon;
        private VisualElement _optionsContainer;

        public BasicDropdown()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicDropdown/BasicDropdown");
            styleSheets.Add(styleSheet);

            AddToClassList("basic-dropdown");

            _button = new VisualElement();
            _button.name = "Button";
            _button.AddToClassList("basic-dropdown-button");

            _label = new Label("Select...");
            _label.name = "Label";
            _label.AddToClassList("basic-dropdown-label");

            _icon = new VisualElement();
            _icon.name = "Icon";
            _icon.AddToClassList("basic-dropdown-icon");

            _optionsContainer = new VisualElement();
            _optionsContainer.name = "OptionsContainer";
            _optionsContainer.AddToClassList("basic-dropdown-options");
            _optionsContainer.style.display = DisplayStyle.None;

            _button.Add(_label);
            _button.Add(_icon);
            Add(_button);
            Add(_optionsContainer);

            _button.RegisterCallback<MouseDownEvent>(evt => ToggleDropdown());

            focusable = true;
            RegisterCallback<FocusOutEvent>(evt => CloseDropdown());
        }

        public void SetOptions(List<string> options)
        {
            _optionsList = options;
            _optionsContainer.Clear();

            foreach (var option in _optionsList)
            {
                Label optionLabel = new Label(option);
                optionLabel.AddToClassList("basic-dropdown-option");
                optionLabel.RegisterCallback<MouseDownEvent>(evt => { SetValue(option, true); });

                _optionsContainer.Add(optionLabel);
            }

            if (string.IsNullOrEmpty(_value) && _optionsList.Count > 0)
            {
                SetValue(_optionsList[0], false);
            }
            else
            {
                _label.text = _value;
            }
        }

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
            _icon.EnableInClassList("basic-dropdown-icon--open", true);
            this.Focus();
        }

        private void CloseDropdown()
        {
            if (!_isOpen) return;
            _isOpen = false;
            _optionsContainer.style.display = DisplayStyle.None;
            _icon.EnableInClassList("basic-dropdown-icon--open", false);
        }
    }
}
