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

        public BasicRadio SelectedRadio =>
            _selectedIndex >= 0 && _selectedIndex < _radios.Count
                ? _radios[_selectedIndex]
                : null;

        public Action<int> OnSelectionChanged { get; set; }

        private readonly List<BasicRadio> _radios = new();
        private int _selectedIndex = -1;
        private bool _initialized;
        private bool _isUpdating;

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

            this.Query<BasicRadio>().ForEach(radio =>
            {
                _radios.Add(radio);
                radio.OnSelectedChanged += selected =>
                {
                    if (selected)
                        OnRadioSelected(radio);
                };
            });

            if (_selectedIndex >= 0)
                SetSelectedIndex(_selectedIndex, false);
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

            if (index < 0 || index >= _radios.Count)
                return;

            _isUpdating = true;
            for (int i = 0; i < _radios.Count; i++)
                _radios[i].SetSelectedInternal(i == index);
            _isUpdating = false;

            if (invokeCallback)
                OnSelectionChanged?.Invoke(_selectedIndex);
        }
    }
}
