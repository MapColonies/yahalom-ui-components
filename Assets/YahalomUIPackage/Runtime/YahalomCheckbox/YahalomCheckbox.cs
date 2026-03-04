using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.YahalomCheckbox
{
    public class YahalomCheckbox : Toggle
    {
        [Header("Yahalom Toggle Sprites")]
        [SerializeField] private Sprite _checkedSprite;
        [SerializeField] private Sprite _uncheckedSprite;
        [SerializeField] private Sprite _disabledSprite;
        [SerializeField] private Sprite _partialSprite;

        [Header("Toggle Hierarchy")]
        public List<YahalomCheckbox> childToggles = new List<YahalomCheckbox>();

        [Header("Yahalom Events")]
        public UnityEvent<bool> onPartialChanged = new UnityEvent<bool>();

        private bool _isPartial;
        private bool _isUpdatingFromParent;

        protected override void OnEnable()
        {
            base.OnEnable();

            onValueChanged.AddListener(HandleValueChanged);
            for (int i = 0; i < childToggles.Count; i++) {
                YahalomCheckbox child = childToggles[i];
                if (child == null) {
                    continue;
                }

                child.onValueChanged.AddListener(HandleChildValueChanged);
                child.onPartialChanged.AddListener(HandleChildPartialChanged);
            }

            RecomputeFromChildren();
            UpdateVisualState();
        }

        protected override void OnDisable()
        {
            onValueChanged.RemoveListener(HandleValueChanged);
            for (int i = 0; i < childToggles.Count; i++) {
                YahalomCheckbox child = childToggles[i];
                if (child == null) {
                    continue;
                }

                child.onValueChanged.RemoveListener(HandleChildValueChanged);
                child.onPartialChanged.RemoveListener(HandleChildPartialChanged);
            }

            base.OnDisable();
        }

        protected override void Awake()
        {
            base.Awake();
            toggleTransition = ToggleTransition.None;
            graphic = null;
        }

        protected override void Start()
        {
            base.Start();
            UpdateVisualState();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            UpdateVisualState();
        }
#endif

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            UpdateVisualState();
        }

        private void HandleValueChanged(bool value)
        {
            for (int i = 0; i < childToggles.Count; i++) {
                YahalomCheckbox child = childToggles[i];
                if (child == null) {
                    continue;
                }

                child.SetIsOnFromParent(value);
            }

            SetIsPartial(false);
            UpdateVisualState();
        }

        private void HandleChildValueChanged(bool _)
        {
            if (_isUpdatingFromParent) return;
            RecomputeFromChildren();
            UpdateVisualState();
        }

        private void HandleChildPartialChanged(bool _)
        {
            if (_isUpdatingFromParent) return;
            RecomputeFromChildren();
            UpdateVisualState();
        }

        private void SetIsPartial(bool value)
        {
            if (_isPartial == value) return;
            _isPartial = value;
            onPartialChanged.Invoke(value);
        }

        private void SetIsOnFromParent(bool value)
        {
            _isUpdatingFromParent = true;
            isOn = value;
            SetIsPartial(false);

            for (int i = 0; i < childToggles.Count; i++) {
                YahalomCheckbox child = childToggles[i];
                if (child == null) {
                    continue;
                }

                child.SetIsOnFromParent(value);
            }

            _isUpdatingFromParent = false;
            UpdateVisualState();
        }

        private void RecomputeFromChildren()
        {
            int totalCount = 0;
            int checkedCount = 0;

            for (int i = 0; i < childToggles.Count; i++) {
                YahalomCheckbox child = childToggles[i];
                if (child == null) {
                    continue;
                }

                if (child._isPartial) {
                    SetIsPartial(true);
                    return;
                }

                totalCount++;
                if (child.isOn) {
                    checkedCount++;
                }
            }

            if (totalCount == 0 || checkedCount == 0 || checkedCount == totalCount) {
                SetIsPartial(false);
                bool allOn = checkedCount == totalCount && totalCount > 0;
                if (isOn != allOn) {
                    isOn = allOn;
                }
            }
            else {
                SetIsPartial(true);
            }
        }

        private void UpdateVisualState()
        {
            Image image = targetGraphic as Image;
            if (image == null) {
                return;
            }

            if (!IsInteractable()) {
                image.sprite = _disabledSprite;
                return;
            }

            if (_isPartial) {
                image.sprite = _partialSprite;
                return;
            }

            image.sprite = isOn ? _checkedSprite : _uncheckedSprite;
        }
    }
}
