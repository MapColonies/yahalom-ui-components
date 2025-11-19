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

        private Color _topColor = new Color(1f, 1f, 1f, 1f);
        private Color _bottomColor = new Color(0f, 0f, 0f, 1f);

        [UxmlAttribute("top-color")]
        public Color TopColor
        {
            get => _topColor;
            set
            {
                _topColor = value;
                if (_gradientBackground != null)
                    _gradientBackground.TopColor = value;
            }
        }

        [UxmlAttribute("bottom-color")]
        public Color BottomColor
        {
            get => _bottomColor;
            set
            {
                _bottomColor = value;
                if (_gradientBackground != null)
                    _gradientBackground.BottomColor = value;
            }
        }

        public bool IsSelected
        {
            get => ClassListContains(SelectedClassName);
            set => EnableInClassList(SelectedClassName, value);
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

            _gradientBackground.TopColor = _topColor;
            _gradientBackground.BottomColor = _bottomColor;
        }
    }
}
