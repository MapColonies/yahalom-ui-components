using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIToolKitPackage.Runtime.HyperlinkButton
{
    [UxmlElement]
    public partial class HyperlinkButton : Button
    {
        public new class UxmlFactory : Button.UxmlFactory
        {
        }

        public HyperlinkButton()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("HyperlinkButton/HyperlinkButton");
            styleSheets.Add(styleSheet);

            AddToClassList("hyperlink-button");
        }
    }
}
