using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicScrollView
{
    [UxmlElement]
    public partial class BasicScrollView : ScrollView
    {
        // We inherit the UxmlFactory from the base ScrollView class.
        // This gives us all its attributes (like scroll-mode) for free.
        public new class UxmlFactory : ScrollView.UxmlFactory {}

        public BasicScrollView()
        {
            // Load stylesheet
            var styleSheet = Resources.Load<StyleSheet>("BasicScrollView/BasicScrollView");
            styleSheets.Add(styleSheet);
            
            // Set base class for styling
            AddToClassList("basic-scroll-view");
        }
    }
}