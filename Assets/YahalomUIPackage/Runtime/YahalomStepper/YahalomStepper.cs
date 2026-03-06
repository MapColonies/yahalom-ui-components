using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.YahalomStepper
{
    public class YahalomStepper : MonoBehaviour
    {
        [SerializeField] private int _value;
        [SerializeField] private int _min;
        [SerializeField] private int _max;

        [Header("References")]
        [SerializeField] private Button _incrementButton;
        [SerializeField] private Button _decrementButton;

        [Tooltip("Input field shown when max is unset (0)")]
        [SerializeField] private TMP_InputField _standardInputField;

        [Tooltip("Input field shown when max is set (non-zero)")]
        [SerializeField] private TMP_InputField _rangeInputField;

        [Tooltip("Text label (child of the range layout) that displays the max value, e.g. \"/10\"")]
        [SerializeField] private TMP_Text _maxLabel;

        [Header("Events")]
        [SerializeField] private UnityEvent<int> _onValueChanged;

        public int Value {
            get => _value;
            set => SetValue(value);
        }

        public int Min {
            get => _min;
            set {
                _min = value;
                SetValue(_value);
            }
        }

        public int Max {
            get => _max;
            set {
                _max = value;
                SetValue(_value);
            }
        }

        public UnityEvent<int> OnValueChanged => _onValueChanged;

        private bool HasRange => _max != 0;

        private TMP_InputField ActiveInputField => HasRange ? _rangeInputField : _standardInputField;

        private void Awake() {
            if (_incrementButton != null) {
                _incrementButton.onClick.AddListener(Increment);
            }

            if (_decrementButton != null) {
                _decrementButton.onClick.AddListener(Decrement);
            }

            if (_standardInputField != null) {
                _standardInputField.onSelect.AddListener(OnInputFieldSelected);
                _standardInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
            }

            if (_rangeInputField != null) {
                _rangeInputField.onSelect.AddListener(OnInputFieldSelected);
                _rangeInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
            }

            UpdateDisplay();
        }

        private void OnDestroy() {
            if (_incrementButton != null) {
                _incrementButton.onClick.RemoveListener(Increment);
            }

            if (_decrementButton != null) {
                _decrementButton.onClick.RemoveListener(Decrement);
            }

            if (_standardInputField != null) {
                _standardInputField.onSelect.RemoveListener(OnInputFieldSelected);
                _standardInputField.onEndEdit.RemoveListener(OnInputFieldEndEdit);
            }

            if (_rangeInputField != null) {
                _rangeInputField.onSelect.RemoveListener(OnInputFieldSelected);
                _rangeInputField.onEndEdit.RemoveListener(OnInputFieldEndEdit);
            }
        }

        private void Increment() {
            SetValue(_value + 1);
        }

        private void Decrement() {
            SetValue(_value - 1);
        }

        private void SetValue(int newValue) {
            newValue = Mathf.Max(newValue, _min);
            if (HasRange) {
                newValue = Mathf.Min(newValue, _max);
            }

            if (_value == newValue) {
                UpdateDisplay();
                return;
            }

            _value = newValue;
            UpdateDisplay();
            _onValueChanged?.Invoke(_value);
        }

        private void OnInputFieldSelected(string text) {
            ActiveInputField.text = _value.ToString();
        }

        private void OnInputFieldEndEdit(string text) {
            if (int.TryParse(text.Trim(), out int result)) {
                SetValue(result);
            }
            else {
                UpdateDisplay();
            }
        }

        private void UpdateDisplay() {
            if (_standardInputField != null) {
                _standardInputField.gameObject.SetActive(!HasRange);
            }

            if (_rangeInputField != null) {
                _rangeInputField.gameObject.SetActive(HasRange);
            }

            if (ActiveInputField != null) {
                ActiveInputField.text = _value.ToString();
            }

            if (_maxLabel != null) {
                _maxLabel.text = HasRange ? $"/{_max}" : string.Empty;
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates() {
            if (_incrementButton != null) {
                _incrementButton.interactable = !HasRange || _value < _max;
            }

            if (_decrementButton != null) {
                _decrementButton.interactable = _value > _min;
            }
        }

#if UNITY_EDITOR
        private void OnValidate() {
            _value = Mathf.Max(_value, _min);
            if (HasRange) {
                _value = Mathf.Min(_value, _max);
            }

            UpdateDisplay();
        }
#endif
    }
}
