using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private List<Toggle> _childToggles = new List<Toggle>();

    private YahalomCheckbox _parentCheckbox;
    private bool _isPartial;
    private bool _lastEffectiveInteractable = true;

    protected override void OnEnable()
    {
        base.OnEnable();

        onValueChanged.AddListener(HandleValueChanged);
        for (int i = 0; i < _childToggles.Count; i++) {
            Toggle child = _childToggles[i];
            if (child == null) {
                continue;
            }

            child.onValueChanged.AddListener(HandleChildValueChanged);
            if (child is YahalomCheckbox childCheckbox) {
                childCheckbox._parentCheckbox = this;
            }
        }

        RecomputeFromChildren();
        UpdateVisualState();
        _lastEffectiveInteractable = IsInteractable();
    }

    protected override void OnDisable()
    {
        onValueChanged.RemoveListener(HandleValueChanged);
        for (int i = 0; i < _childToggles.Count; i++) {
            Toggle child = _childToggles[i];
            if (child == null) {
                continue;
            }

            child.onValueChanged.RemoveListener(HandleChildValueChanged);
            if (child is YahalomCheckbox childCheckbox) {
                childCheckbox._parentCheckbox = null;
            }
        }

        base.OnDisable();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        RecomputeFromChildren();
        UpdateVisualState();
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        UpdateVisualState();
    }

    private void HandleValueChanged(bool isOn)
    {
        PropagateToChildren(isOn);
        UpdateVisualState();
    }

    private void HandleChildValueChanged(bool _)
    {
        RecomputeFromChildren();
        UpdateVisualState();
        PropagateStateToParent();
    }

    private void PropagateToChildren(bool value)
    {
        for (int i = 0; i < _childToggles.Count; i++) {
            Toggle child = _childToggles[i];
            if (child == null) {
                continue;
            }

            if (child is YahalomCheckbox childCheckbox) {
                childCheckbox.SetIsOnFromParent(value);
            }
            else {
                child.isOn = value;
            }
        }

        _isPartial = false;
    }

    private void SetIsOnFromParent(bool value)
    {
        isOn = value;
        PropagateToChildren(value);
        UpdateVisualState();
    }

    private void RecomputeFromChildren()
    {
        if (_childToggles.Count == 0) {
            _isPartial = false;
            return;
        }

        bool hasValue = false;
        bool firstValue = false;
        bool allSame = true;

        for (int i = 0; i < _childToggles.Count; i++) {
            Toggle child = _childToggles[i];
            if (child == null || !child.IsInteractable()) {
                continue;
            }

            if (child is YahalomCheckbox childCheckbox && childCheckbox._isPartial) {
                _isPartial = true;
                return;
            }

            if (!hasValue) {
                hasValue = true;
                firstValue = child.isOn;
            }
            else if (child.isOn != firstValue) {
                allSame = false;
            }
        }

        if (!hasValue) {
            _isPartial = false;
            return;
        }

        if (allSame) {
            _isPartial = false;
            if (isOn != firstValue) {
                isOn = firstValue;
            }
        }
        else {
            _isPartial = true;
        }
    }

    private void PropagateStateToParent()
    {
        if (_parentCheckbox == null) {
            return;
        }

        _parentCheckbox.RecomputeFromChildren();
        _parentCheckbox.UpdateVisualState();
        _parentCheckbox.PropagateStateToParent();
    }

    private void UpdateVisualState()
    {
        Image image = targetGraphic as Image;
        if (image == null) {
            return;
        }

        bool isInteractable = IsInteractable();
        if (isInteractable != _lastEffectiveInteractable) {
            UpdateChildrenInteractable(isInteractable);
            _lastEffectiveInteractable = isInteractable;
        }

        if (!isInteractable) {
            image.sprite = _disabledSprite;
            return;
        }

        if (_isPartial) {
            image.sprite = _partialSprite;
            return;
        }

        image.sprite = isOn ? _checkedSprite : _uncheckedSprite;
    }

    private void UpdateChildrenInteractable(bool interactable)
    {
        for (int i = 0; i < _childToggles.Count; i++) {
            Toggle child = _childToggles[i];
            if (child == null) {
                continue;
            }

            child.interactable = interactable;
            if (child is YahalomCheckbox childCheckbox) {
                childCheckbox.UpdateChildrenInteractable(interactable);
            }
        }
    }
}

}