using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIToolKitPackage.Runtime.BasicCheckbox
{
    [UxmlElement]
    public partial class BasicCheckbox : Toggle
    {
        public new class UxmlFactory : Toggle.UxmlFactory
        {
        }

        public BasicCheckbox()
        {
            StyleSheet styleSheet = Resources.Load<StyleSheet>("BasicCheckbox/BasicCheckbox");

            if (styleSheet != null)
            {
                styleSheets.Add(styleSheet);
            }
            else
            {
                Debug.LogError("[BasicCheckbox] Could not load 'BasicCheckbox/BasicCheckbox.uss'.");
            }

            AddToClassList("basic-checkbox");
        }
    }
}
