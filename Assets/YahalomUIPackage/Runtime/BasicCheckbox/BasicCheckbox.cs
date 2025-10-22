using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicCheckbox
{
    [UxmlElement]
    public partial class BasicCheckbox : Toggle
    {
        // We inherit the UxmlFactory from the base Toggle class.
        // This automatically gives us the 'text' and 'value' attributes
        // in the UI Builder.
        public new class UxmlFactory : Toggle.UxmlFactory {}

        // The default constructor is all we need
        public BasicCheckbox()
        {
            // Load our custom stylesheet
            var styleSheet = Resources.Load<StyleSheet>("BasicCheckbox/BasicCheckbox");
            styleSheets.Add(styleSheet);
            
            // Add a base class so our USS can target it
            AddToClassList("basic-checkbox");
        }
    }
}