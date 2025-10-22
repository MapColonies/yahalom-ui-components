using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.BasicScrollView
{
    [UxmlElement]
    public partial class BasicScrollView : ScrollView
    {
        public new class UxmlFactory : ScrollView.UxmlFactory
        {
        }

        public BasicScrollView()
        {
            var styleSheet = Resources.Load<StyleSheet>("BasicScrollView/BasicScrollView");
            styleSheets.Add(styleSheet);
            AddToClassList("basic-scroll-view");
        }
    }
}
