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

        private Texture2D _iconImage;

        private Color _topColor = Color.white;
        private Color _bottomColor = Color.black;

        private Color _hoverTopColor = Color.white;
        private Color _hoverBottomColor = Color.black;

        private Color _selectedTopColor = Color.white;
        private Color _selectedBottomColor = Color.black;

        private Color _disabledTopColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        private Color _disabledBottomColor = new Color(0.2f, 0.2f, 0.2f, 1f);

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

            if (!enabledInHierarchy || !enabledSelf)
            {
                top = _disabledTopColor;
                bottom = _disabledBottomColor;
            }
            else if (IsSelected)
            {
                top = _selectedTopColor;
                bottom = _selectedBottomColor;
            }
            else if (_isPointerOver)
            {
                top = _hoverTopColor;
                bottom = _hoverBottomColor;
            }
            else
            {
                top = _topColor;
                bottom = _bottomColor;
            }

            _gradientBackground.TopColor = top;
            _gradientBackground.BottomColor = bottom;
        }
    }
}
