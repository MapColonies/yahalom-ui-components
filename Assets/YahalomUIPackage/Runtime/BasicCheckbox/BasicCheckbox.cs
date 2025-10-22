using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicCheckbox
{
    [UxmlElement]
    public partial class BasicCheckbox : Toggle
    {
        public new class UxmlFactory : Toggle.UxmlFactory
        {
        }

        public BasicCheckbox()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicCheckbox/BasicCheckbox");
            styleSheets.Add(styleSheet);
            AddToClassList("basic-checkbox");
        }
    }
}
