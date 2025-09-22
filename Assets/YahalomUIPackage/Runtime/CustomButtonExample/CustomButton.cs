using UnityEngine;
using UnityEngine.UIElements;

namespace YahalomUIPackage.Runtime.CustomButtonExample
{
    public class CustomButton :Button
    {
        public new class UxmlFactory : UxmlFactory<CustomButton, UxmlTraits> { }

        public CustomButton()
        {
            var styleSheet = Resources.Load<StyleSheet>("CustomButtonExample/CustomButtonExample"); 
            styleSheets.Add(styleSheet);
            
            AddToClassList("custom-button");
        }
    }
}