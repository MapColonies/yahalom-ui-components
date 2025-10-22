using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicInputField
{
    [UxmlElement]
    public partial class BasicInputField : VisualElement
    {
#region UXML Factory

        public new class UxmlFactory : UxmlFactory<BasicInputField, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription
                { name = "text", defaultValue = "Input Label" };

            UxmlStringAttributeDescription m_Value = new UxmlStringAttributeDescription
                { name = "value", defaultValue = "" };

            UxmlStringAttributeDescription m_Placeholder = new UxmlStringAttributeDescription
                { name = "placeholder", defaultValue = "Enter text..." };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as BasicInputField;

                ate.Text = m_Text.GetValueFromBag(bag, cc);
                ate.Value = m_Value.GetValueFromBag(bag, cc);
                ate.Placeholder = m_Placeholder.GetValueFromBag(bag, cc);
            }
        }

#endregion

        public Action<string> OnValueChanged { get; set; }

        [UxmlAttribute("text")]
        public string Text
        {
            get => _label.text;
            set => _label.text = value;
        }

        [UxmlAttribute("value")]
        public string Value
        {
            get => _textField.value;
            set => _textField.value = value;
        }

        [UxmlAttribute("placeholder")]
        public string Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;

                if (_placeholderElement != null)
                {
                    _placeholderElement.text = _placeholder;
                }
            }
        }

        private Label _label;
        private TextField _textField;
        private TextElement _placeholderElement;
        private string _placeholder;

        public BasicInputField()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicInputField/BasicInputField");
            styleSheets.Add(styleSheet);

            AddToClassList("basic-input-field");

            _label = new Label();
            _label.AddToClassList("basic-input-field-label");

            _textField = new TextField();
            _textField.AddToClassList("basic-input-field-input");

            _placeholderElement = _textField.Q<TextElement>(className: "unity-placeholder");

            Add(_label);
            Add(_textField);

            _textField.RegisterValueChangedCallback(OnInternalValueChanged);
        }

        private void OnInternalValueChanged(ChangeEvent<string> evt)
        {
            OnValueChanged?.Invoke(evt.newValue);
        }
    }
}
