using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIToolKitPackage.Runtime.NumberStepper
{
    [UxmlElement]
    public partial class NumberStepper : VisualElement
    {
        [UxmlAttribute]
        public int Min
        {
            get => _min;
            set
            {
                _min = value;
                if (_value < _min)
                    SetValue(_min);
            }
        }

        [UxmlAttribute]
        public int Max
        {
            get => _max;
            set
            {
                _max = value;
                if (_value > _max)
                    SetValue(_max);
                else
                    UpdateTexts();
            }
        }

        [UxmlAttribute]
        public int Value
        {
            get => _value;
            set => SetValue(value);
        }

        [UxmlAttribute] public string LabelTemplate { get; set; } = "מתוך {0}";

        public Action<int> OnValueChanged { get; set; }

        private int _min = 0;
        private int _max = 288;
        private int _value = 0;

        private Button _plusButton;
        private Button _minusButton;
        private VisualElement _centerContainer;
        private Label _centerLabel;

        private VisualElement _valueBubbleContainer;
        private Label _valueBubbleLabel;

        public NumberStepper()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("NumberStepper/NumberStepper");
            styleSheets.Add(styleSheet);

            AddToClassList("numberstepper");

            _plusButton = new Button { text = "+" };
            _plusButton.name = "PlusButton";
            _plusButton.AddToClassList("numberstepper__segment");
            _plusButton.AddToClassList("numberstepper__segment--left");
            _plusButton.RegisterCallback<ClickEvent>(_ => SetValue(_value + 1));

            _centerContainer = new VisualElement();
            _centerContainer.name = "Center";
            _centerContainer.AddToClassList("numberstepper__center");

            _centerLabel = new Label();
            _centerLabel.name = "CenterLabel";
            _centerLabel.AddToClassList("numberstepper__text");

            _valueBubbleContainer = new VisualElement();
            _valueBubbleContainer.name = "ValueBubble";
            _valueBubbleContainer.AddToClassList("numberstepper__bubble");

            _valueBubbleLabel = new Label();
            _valueBubbleLabel.name = "ValueBubbleText";
            _valueBubbleLabel.AddToClassList("numberstepper__bubble-text");

            _valueBubbleContainer.Add(_valueBubbleLabel);

            _centerContainer.Add(_valueBubbleContainer);
            _centerContainer.Add(_centerLabel);

            _minusButton = new Button { text = "−" };
            _minusButton.name = "MinusButton";
            _minusButton.AddToClassList("numberstepper__segment");
            _minusButton.AddToClassList("numberstepper__segment--right");
            _minusButton.RegisterCallback<ClickEvent>(_ => SetValue(_value - 1));

            Add(_plusButton);
            Add(_centerContainer);
            Add(_minusButton);

            UpdateTexts();
        }

        private void SetValue(int newValue)
        {
            newValue = Mathf.Clamp(newValue, _min, _max);

            if (_value == newValue)
                return;

            _value = newValue;
            UpdateTexts();
            OnValueChanged?.Invoke(_value);
        }

        private void UpdateTexts()
        {
            if (_centerLabel != null)
                _centerLabel.text = string.Format(LabelTemplate, _max);

            if (_valueBubbleLabel != null)
                _valueBubbleLabel.text = _value.ToString();
        }
    }
}
