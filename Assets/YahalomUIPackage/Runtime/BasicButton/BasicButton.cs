using UnityEngine;
using UnityEngine.UIElements;
using YahalomUIPackage.Runtime.Effects;

namespace YahalomUIPackage.Runtime.BasicButton
{
    [UxmlElement]
    public partial class BasicButton : Button
    {
        public const string SelectedClassName = "basic-button--selected";

        private GradientBackgroundElement _gradientBackground;
        private VisualElement _iconElement;
        private Label _textElement;

        private Texture2D _iconImage;

        private Color _topColor = Color.white;
        private Color _bottomColor = Color.black;

        private Color _hoverTopColor = Color.white;
        private Color _hoverBottomColor = Color.black;

        private Color _selectedTopColor = Color.white;
        private Color _selectedBottomColor = Color.black;

        private Color _disabledTopColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        private Color _disabledBottomColor = new Color(0.2f, 0.2f, 0.2f, 1f);

        private Color _textColor = Color.white;
        private Color _hoverTextColor = Color.white;
        private Color _selectedTextColor = Color.white;
        private Color _disabledTextColor = new Color(0.7f, 0.7f, 0.7f, 1f);

        private bool _isPointerOver;

        [UxmlAttribute("icon-image")]
        public Texture2D IconImage
        {
            get => _iconImage;
            set
            {
                _iconImage = value;

                if (_iconElement == null)
                    return;

                if (value == null)
                    _iconElement.style.backgroundImage = StyleKeyword.None;
                else
                    _iconElement.style.backgroundImage = new StyleBackground(value);
            }
        }
        
        [UxmlAttribute("top-color")]
        public Color TopColor
        {
            get => _topColor;
            set { _topColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("bottom-color")]
        public Color BottomColor
        {
            get => _bottomColor;
            set { _bottomColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("hover-top-color")]
        public Color HoverTopColor
        {
            get => _hoverTopColor;
            set { _hoverTopColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("hover-bottom-color")]
        public Color HoverBottomColor
        {
            get => _hoverBottomColor;
            set { _hoverBottomColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("selected-top-color")]
        public Color SelectedTopColor
        {
            get => _selectedTopColor;
            set { _selectedTopColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("selected-bottom-color")]
        public Color SelectedBottomColor
        {
            get => _selectedBottomColor;
            set { _selectedBottomColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("disabled-top-color")]
        public Color DisabledTopColor
        {
            get => _disabledTopColor;
            set { _disabledTopColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("disabled-bottom-color")]
        public Color DisabledBottomColor
        {
            get => _disabledBottomColor;
            set { _disabledBottomColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("label-text")]
        public string LabelText
        {
            get => _textElement != null ? _textElement.text : string.Empty;
            set
            {
                if (_textElement != null)
                    _textElement.text = value;
            }
        }
        
        [UxmlAttribute("text-color")]
        public Color TextColor
        {
            get => _textColor;
            set { _textColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("hover-text-color")]
        public Color HoverTextColor
        {
            get => _hoverTextColor;
            set { _hoverTextColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("selected-text-color")]
        public Color SelectedTextColor
        {
            get => _selectedTextColor;
            set { _selectedTextColor = value; UpdateVisualStateInternal(); }
        }

        [UxmlAttribute("disabled-text-color")]
        public Color DisabledTextColor
        {
            get => _disabledTextColor;
            set { _disabledTextColor = value; UpdateVisualStateInternal(); }
        }

        public bool IsSelected
        {
            get => ClassListContains(SelectedClassName);
            set
            {
                EnableInClassList(SelectedClassName, value);
                UpdateVisualStateInternal();
            }
        }

        public BasicButton()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicButton/BasicButton");
            if (styleSheet != null)
                styleSheets.Add(styleSheet);

            AddToClassList("basic-button");

            _gradientBackground = new GradientBackgroundElement
            {
                name = "basic-button-gradient-bg",
                pickingMode = PickingMode.Ignore
            };
            _gradientBackground.StretchToParentSize();
            hierarchy.Insert(0, _gradientBackground);

            _iconElement = new VisualElement
            {
                name = "basic-button-icon",
                pickingMode = PickingMode.Ignore
            };
            _iconElement.AddToClassList("basic-button__icon");
            hierarchy.Add(_iconElement);

            _textElement = new Label
            {
                name = "basic-button-label",
                pickingMode = PickingMode.Ignore
            };
            _textElement.AddToClassList("basic-button__label");
            hierarchy.Add(_textElement);

            RegisterCallback<PointerEnterEvent>(_ =>
            {
                _isPointerOver = true;
                UpdateVisualStateInternal();
            });
            RegisterCallback<PointerLeaveEvent>(_ =>
            {
                _isPointerOver = false;
                UpdateVisualStateInternal();
            });

            RegisterCallback<AttachToPanelEvent>(_ => UpdateVisualStateInternal());

            UpdateVisualStateInternal();
        }

        public void RefreshVisualState()
        {
            UpdateVisualStateInternal();
        }

        private void UpdateVisualStateInternal()
        {
            if (_gradientBackground == null)
                return;

            Color top;
            Color bottom;
            Color textColor;

            if (!enabledInHierarchy || !enabledSelf)
            {
                top = _disabledTopColor;
                bottom = _disabledBottomColor;
                textColor = _disabledTextColor;
            }
            else if (IsSelected)
            {
                top = _selectedTopColor;
                bottom = _selectedBottomColor;
                textColor = _selectedTextColor;
            }
            else if (_isPointerOver)
            {
                top = _hoverTopColor;
                bottom = _hoverBottomColor;
                textColor = _hoverTextColor;
            }
            else
            {
                top = _topColor;
                bottom = _bottomColor;
                textColor = _textColor;
            }

            _gradientBackground.TopColor = top;
            _gradientBackground.BottomColor = bottom;

            if (_textElement != null)
                _textElement.style.color = textColor;
        }
    }
}
