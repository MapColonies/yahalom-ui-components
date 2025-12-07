using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicTab
{
    [UxmlElement]
    public partial class BasicTab : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<BasicTab>
        {
        }

        private Label _label;
        private VisualElement _underline;

        private string _text = "Tab";
        private bool _isSelected;

        [UxmlAttribute]
        public string text
        {
            get => _text;
            set
            {
                _text = value;

                if (_label != null)
                {
                    _label.text = value;
                }
            }
        }

        [UxmlAttribute("is-selected")]
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UpdateVisualState();
            }
        }

        public BasicTab()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("BasicTab/BasicTab");

            if (styleSheet != null)
            {
                styleSheets.Add(styleSheet);
            }
            else
            {
                Debug.LogError("[BasicTab] Could not load 'BasicTab/BasicTab.uss'.");
            }

            AddToClassList("basic-tab");

            _label = new Label();
            _label.AddToClassList("basic-tab__label");

            _underline = new VisualElement();
            _underline.AddToClassList("basic-tab__underline");

            hierarchy.Add(_label);
            hierarchy.Add(_underline);

            RegisterCallback<ClickEvent>(_ => { isSelected = true; });

            _label.text = _text;
            UpdateVisualState();
        }

        private void UpdateVisualState()
        {
            if (_isSelected)
            {
                AddToClassList("basic-tab--selected");
            }
            else
            {
                RemoveFromClassList("basic-tab--selected");
            }
        }
    }
}
