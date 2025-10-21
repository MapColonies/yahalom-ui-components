using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicSlider
{
    [UxmlElement]
    public partial class BasicSlider : VisualElement
    {
        #region UXML Factory
        // We include the Factory and Traits to provide default values
        // for our UXML attributes, which prevents null-reference errors
        // in the UI Builder when the element is first added.
        public new class UxmlFactory : UxmlFactory<BasicSlider, UxmlTraits> {}
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            // Define the attributes and their default values
            UxmlStringAttributeDescription m_Text = new UxmlStringAttributeDescription { name = "text", defaultValue = "Slider" };
            UxmlFloatAttributeDescription m_LowValue = new UxmlFloatAttributeDescription { name = "low-value", defaultValue = 0f };
            UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription { name = "high-value", defaultValue = 1f };
            UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription { name = "value", defaultValue = 0f };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as BasicSlider;

                // Read values from UXML, using defaults if not present
                ate.Text = m_Text.GetValueFromBag(bag, cc);
                ate.LowValue = m_LowValue.GetValueFromBag(bag, cc);
                ate.HighValue = m_HighValue.GetValueFromBag(bag, cc);
                ate.Value = m_Value.GetValueFromBag(bag, cc);
            }
        }
        #endregion

        /// <summary>
        /// Fires when the selected value is changed by the user.
        /// </summary>
        public Action<float> OnValueChanged { get; set; }

        [UxmlAttribute("text")]
        public string Text { get => _label.text; set => _label.text = value; }

        [UxmlAttribute("low-value")]
        public float LowValue
        {
            get => _lowValue;
            set
            {
                _lowValue = value;
                // Re-set the value to clamp it to the new range and update visuals
                SetValue(Mathf.Clamp(_value, _lowValue, _highValue), false);
            }
        }

        [UxmlAttribute("high-value")]
        public float HighValue
        {
            get => _highValue;
            set
            {
                _highValue = value;
                // Re-set the value to clamp it to the new range and update visuals
                SetValue(Mathf.Clamp(_value, _lowValue, _highValue), false);
            }
        }
        
        [UxmlAttribute("value")]
        public float Value { get => _value; set => SetValue(value, true); }


        private Label _label;
        private VisualElement _sliderContainer;
        private VisualElement _track;
        private VisualElement _progress;
        private VisualElement _knob;

        private float _lowValue = 0f;
        private float _highValue = 1f;
        private float _value = 0f;
        private bool _isDragging = false;

        public BasicSlider()
        {
            // Load stylesheet
            var styleSheet = Resources.Load<StyleSheet>("BasicSlider/BasicSlider");
            styleSheets.Add(styleSheet);
            
            // Set base class
            AddToClassList("basic-slider");

            // --- Create Element Hierarchy ---
            _label = new Label();
            _label.AddToClassList("basic-slider-label");
            
            _sliderContainer = new VisualElement();
            _sliderContainer.AddToClassList("basic-slider-container");
            
            _track = new VisualElement();
            _track.AddToClassList("basic-slider-track");
            
            _progress = new VisualElement();
            _progress.AddToClassList("basic-slider-progress");
            
            _knob = new VisualElement();
            _knob.AddToClassList("basic-slider-knob");
            
            // --- Assemble Hierarchy ---
            Add(_label);
            Add(_sliderContainer);
            _sliderContainer.Add(_track);
            _track.Add(_progress);
            _sliderContainer.Add(_knob); // Knob is a sibling of the track

            // --- Register Callbacks ---
            _sliderContainer.RegisterCallback<PointerDownEvent>(OnPointerDown);
            _sliderContainer.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            _sliderContainer.RegisterCallback<PointerUpEvent>(OnPointerUp);
            
            // Call once to set initial visual state
            UpdateVisuals();
        }

        /// <summary>
        /// Sets the current value of the slider.
        /// </summary>
        /// <param name="newValue">The new value to set.</param>
        /// <param name="notify">If true, invokes the OnValueChanged event.</param>
        public void SetValue(float newValue, bool notify = true)
        {
            float clampedValue = Mathf.Clamp(newValue, _lowValue, _highValue);
            bool valueChanged = !Mathf.Approximately(_value, clampedValue);
            
            _value = clampedValue;
            UpdateVisuals();

            if (notify && valueChanged)
            {
                OnValueChanged?.Invoke(_value);
            }
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            _isDragging = true;
            // Capture the pointer on the container, not the clicked target
            _sliderContainer.CapturePointer(evt.pointerId); 
            
            // Calculate position relative to the container, not the target
            float pointerX = _sliderContainer.WorldToLocal(evt.position).x;
            CalculateValueFromPointer(pointerX);
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isDragging || !_isDragging) return; // Check _isDragging
            
            // Calculate position relative to the container
            float pointerX = _sliderContainer.WorldToLocal(evt.position).x;
            CalculateValueFromPointer(pointerX);
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!_isDragging) return;
            _isDragging = false;
            // Release the pointer from the container
            _sliderContainer.ReleasePointer(evt.pointerId); 
        }

        private void CalculateValueFromPointer(float pointerX)
        {
            // Use layout.width because it's the actual rendered width
            float containerWidth = _sliderContainer.layout.width;
            if (containerWidth <= 0) return;

            float percent = Mathf.Clamp01(pointerX / containerWidth);
            float newValue = _lowValue + (_highValue - _lowValue) * percent;
            
            SetValue(newValue, true);
        }

        private void UpdateVisuals()
        {
            // Avoid divide by zero if low and high are the same
            if (Mathf.Approximately(_highValue, _lowValue))
            {
                _progress.style.width = Length.Percent(0);
                _knob.style.left = Length.Percent(0);
                return;
            }

            float percent = (_value - _lowValue) / (_highValue - _lowValue);
            
            _progress.style.width = Length.Percent(percent * 100);
            _knob.style.left = Length.Percent(percent * 100);
        }
    }
}
