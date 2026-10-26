using UnityEngine;
using UnityEngine.UI;

namespace YahalomUIPackage.Runtime.YahalomRadioToggle
{

public class YahalomRadioToggle : Toggle
{
    [Header("Yahalom Toggle Sprites")]
    [SerializeField] private Sprite _checkedSprite;
    [SerializeField] private Sprite _uncheckedSprite;
    [SerializeField] private Sprite _disabledCheckedSprite;
    [SerializeField] private Sprite _disabledUncheckedSprite;

    [SerializeField] private Image _toggleImage;

    protected override void Awake()
    {
        base.Awake();

        if (_toggleImage == null) {
            _toggleImage = GetComponentInChildren<Image>();
        }

        onValueChanged.AddListener(OnValueChanged);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(bool isOn)
    {
        UpdateSprite();
    }

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (_toggleImage == null) {
            return;
        }

        if (!interactable) {
            _toggleImage.sprite = isOn ? _disabledCheckedSprite : _disabledUncheckedSprite;
        } else {
            _toggleImage.sprite = isOn ? _checkedSprite : _uncheckedSprite;
        }
    }
}

}
