using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIToolKitPackage.Runtime.AxisScrollbar
{
    [UxmlElement]
    public partial class AxisScrollbar : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<AxisScrollbar>
        {
        }

        private readonly VisualElement _track;
        private readonly VisualElement _ticksRow;
        private readonly VisualElement _labelsRow;

        private readonly List<VisualElement> _tickElements = new();
        private readonly List<Label> _labelElements = new();

        private string[] _labels =
        {
            "0.25", "0.50", "0.75", "1", "x2", "x3", "x4"
        };

        private int _selectedIndex = 1;

        private bool _isDragging;
        private int _activePointerId = -1;

        [UxmlAttribute("selected-index")]
        public int selectedIndex
        {
            get => _selectedIndex;
            set => SetSelectedIndex(value, false);
        }

        [UxmlAttribute]
        public string labels
        {
            get => string.Join(",", _labels);
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _labels = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }

                BuildTicksAndLabels();
                SetSelectedIndex(_selectedIndex, true);
            }
        }

        public event Action<int, string> SelectionChanged;

        public AxisScrollbar()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("AxisScrollbar/AxisScrollbar");

            if (styleSheet != null)
            {
                styleSheets.Add(styleSheet);
            }
            else
            {
                Debug.LogError("[AxisScrollbar] Could not load 'AxisScrollbar/AxisScrollbar.uss'.");
            }

            AddToClassList("axis-scrollbar");

            style.width = 217;
            style.minWidth = 217;
            style.maxWidth = 217;

            style.height = 43;
            style.minHeight = 43;
            style.maxHeight = 43;

            _track = new VisualElement();
            _track.AddToClassList("axis-scrollbar__track");

            _ticksRow = new VisualElement();
            _ticksRow.AddToClassList("axis-scrollbar__ticks-row");

            _labelsRow = new VisualElement();
            _labelsRow.AddToClassList("axis-scrollbar__labels-row");

            hierarchy.Add(_track);
            hierarchy.Add(_ticksRow);
            hierarchy.Add(_labelsRow);

            BuildTicksAndLabels();
            SetSelectedIndex(_selectedIndex, true);

            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerMoveEvent>(OnPointerMove);
            RegisterCallback<PointerUpEvent>(OnPointerUp);
            RegisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            schedule.Execute(() => UpdateLabelPositions());
        }

        private void BuildTicksAndLabels()
        {
            _ticksRow.Clear();
            _labelsRow.Clear();
            _tickElements.Clear();
            _labelElements.Clear();

            if (_labels == null || _labels.Length == 0)
                return;

            for (int i = 0; i < _labels.Length; i++)
            {
                int index = i;

                VisualElement tick = new VisualElement();
                tick.AddToClassList("axis-scrollbar__tick");

                VisualElement indicator = new VisualElement();
                indicator.AddToClassList("axis-scrollbar__indicator");
                tick.Add(indicator);

                tick.RegisterCallback<ClickEvent>(_ => SetSelectedIndex(index, false));

                _ticksRow.Add(tick);
                _tickElements.Add(tick);

                Label label = new Label(_labels[i]);
                label.AddToClassList("axis-scrollbar__label");
                label.RegisterCallback<ClickEvent>(_ => SetSelectedIndex(index, false));

                _labelsRow.Add(label);
                _labelElements.Add(label);
            }

            schedule.Execute(() => UpdateLabelPositions());
        }

        private void UpdateLabelPositions()
        {
            if (_tickElements.Count != _labelElements.Count)
                return;

            for (int i = 0; i < _tickElements.Count; i++)
            {
                VisualElement tick = _tickElements[i];
                Label label = _labelElements[i];

                if (tick.layout.width > 0)
                {
                    float tickCenterX = tick.layout.x + tick.layout.width / 2f;
                    label.style.left = tickCenterX;
                }
            }
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0) return;

            _isDragging = true;
            _activePointerId = evt.pointerId;

            this.CapturePointer(evt.pointerId);

            UpdateFromWorldPosition(evt.position);
            evt.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isDragging || evt.pointerId != _activePointerId)
                return;

            UpdateFromWorldPosition(evt.position);
            evt.StopPropagation();
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!_isDragging || evt.pointerId != _activePointerId)
                return;

            _isDragging = false;
            this.ReleasePointer(evt.pointerId);

            evt.StopPropagation();
        }

        private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
        {
            _isDragging = false;
            _activePointerId = -1;
        }

        private void UpdateFromWorldPosition(Vector2 worldPos)
        {
            if (_labels == null || _labels.Length == 0)
                return;

            Rect ticksBounds = _ticksRow.worldBound;
            if (ticksBounds.width <= 0f)
                return;

            float fraction = Mathf.InverseLerp(ticksBounds.xMin, ticksBounds.xMax, worldPos.x);
            float rawIndex = fraction * (_labels.Length - 1);
            int index = Mathf.Clamp(Mathf.RoundToInt(rawIndex), 0, _labels.Length - 1);

            SetSelectedIndex(index, false);
        }

        private void SetSelectedIndex(int index, bool fromInternal)
        {
            if (_labels == null || _labels.Length == 0)
                return;

            int clamped = Mathf.Clamp(index, 0, _labels.Length - 1);
            if (clamped == _selectedIndex && !fromInternal)
                return;

            int old = _selectedIndex;
            _selectedIndex = clamped;

            UpdateTickVisuals();

            ChangeEvent<int> evt = ChangeEvent<int>.GetPooled(old, _selectedIndex);
            evt.target = this;
            SendEvent(evt);

            SelectionChanged?.Invoke(_selectedIndex, _labels[_selectedIndex]);
        }

        private void UpdateTickVisuals()
        {
            for (int i = 0; i < _tickElements.Count; i++)
            {
                bool isSelected = i == _selectedIndex;
                _tickElements[i].EnableInClassList("axis-scrollbar__tick--selected", isSelected);
            }
        }
    }
}
