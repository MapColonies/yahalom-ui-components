using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicRadio
{
    [UxmlElement]
    public partial class BasicRadioGroup : VisualElement
    {
        [UxmlAttribute]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetSelectedIndex(value, true);
        }

        [UxmlAttribute]
        public List<string> Choices
        {
            get => _choices;
            set
            {
                _choices = value ?? new List<string>();

                if (_initialized)
                {
                    RebuildFromChoices();
                }
            }
        }

        public BasicRadio SelectedRadio =>
            _selectedIndex >= 0 && _selectedIndex < _radios.Count
                ? _radios[_selectedIndex]
                : null;

        public Action<int> OnSelectionChanged { get; set; }

        private readonly List<BasicRadio> _radios = new();
        private int _selectedIndex = -1;
        private bool _initialized;
        private bool _isUpdating;

        private List<string> _choices = new();

        public BasicRadioGroup()
        {
            AddToClassList("basic-radio-group");
            RegisterCallback<AttachToPanelEvent>(_ => Initialize());
        }

        private void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;
            _radios.Clear();

            this.Query<BasicRadio>().ForEach(RegisterRadio);

            if (_radios.Count == 0 && _choices != null && _choices.Count > 0)
            {
                RebuildFromChoices();
            }
            else
            {
                if (_selectedIndex >= 0)
                    ApplySelectedIndex(false);
            }
        }

        private void RegisterRadio(BasicRadio radio)
        {
            if (radio == null)
                return;

            _radios.Add(radio);
            radio.OnSelectedChanged += selected =>
            {
                if (selected)
                    OnRadioSelected(radio);
            };
        }

        private void RebuildFromChoices()
        {
            Clear();
            _radios.Clear();

            if (_choices == null || _choices.Count == 0)
                return;

            foreach (string choice in _choices)
            {
                BasicRadio radio = new BasicRadio
                {
                    Text = choice
                };

                Add(radio);
                RegisterRadio(radio);
            }

            if (_selectedIndex >= 0)
                ApplySelectedIndex(false);
        }

        private void OnRadioSelected(BasicRadio radio)
        {
            if (_isUpdating)
                return;

            int index = _radios.IndexOf(radio);
            if (index < 0)
                return;

            _isUpdating = true;
            for (int i = 0; i < _radios.Count; i++)
                _radios[i].SetSelectedInternal(i == index);
            _isUpdating = false;

            if (_selectedIndex != index)
            {
                _selectedIndex = index;
                OnSelectionChanged?.Invoke(_selectedIndex);
            }
        }

        private void SetSelectedIndex(int index, bool invokeCallback)
        {
            _selectedIndex = index;

            if (!_initialized)
                return;

            ApplySelectedIndex(invokeCallback);
        }

        private void ApplySelectedIndex(bool invokeCallback)
        {
            if (_selectedIndex < 0 || _selectedIndex >= _radios.Count)
                return;

            _isUpdating = true;
            for (int i = 0; i < _radios.Count; i++)
                _radios[i].SetSelectedInternal(i == _selectedIndex);
            _isUpdating = false;

            if (invokeCallback)
                OnSelectionChanged?.Invoke(_selectedIndex);
        }
    }
}
