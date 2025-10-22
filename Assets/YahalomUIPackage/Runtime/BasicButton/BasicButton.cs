using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicButton
{
    [UxmlElement]
    public partial class BasicButton : Button
    {
        // We inherit the UxmlFactory from the base Button class.
        // This automatically gives us the 'text' attribute and other
        // standard button properties in the UI Builder.
        public new class UxmlFactory : Button.UxmlFactory {}

        // The default constructor is all we need
        public BasicButton()
        {
            // Load our custom stylesheet
            var styleSheet = Resources.Load<StyleSheet>("BasicButton/BasicButton");
            styleSheets.Add(styleSheet);
            
            // Add a base class so our USS can target it
            AddToClassList("basic-button");
        }
    }
}