using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicButton
{
    [UxmlElement]
    public partial class BasicButton : Button
    {
        public new class UxmlFactory : Button.UxmlFactory
        {
        }

        public BasicButton()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicButton/BasicButton");
            styleSheets.Add(styleSheet);
            AddToClassList("basic-button");
        }
    }
}
