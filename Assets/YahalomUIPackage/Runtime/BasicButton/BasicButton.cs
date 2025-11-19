using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicButton
{
    [UxmlElement]
    public partial class BasicButton : Button
    {
        public const string SelectedClassName = "basic-button--selected";

        public new class UxmlFactory : Button.UxmlFactory { }

        public bool IsSelected
        {
            get => ClassListContains(SelectedClassName);
            set => EnableInClassList(SelectedClassName, value);
        }

        public BasicButton()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicButton/BasicButton");
            styleSheets.Add(styleSheet);
            AddToClassList("basic-button");
        }
    }
}
